using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniHTC.Data;
using MiniHTC.Models;
using MiniHTC.Services;
using Serilog;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
FleetObs.ConfigureLogger("minihtc");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");

var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=minihtc.db";
builder.Services.AddDbContext<AppDbContext>(o =>
{
    if (DbUtil.IsPostgres(conn)) o.UseNpgsql(DbUtil.ToNpgsql(conn));
    else o.UseSqlite(conn);
});
builder.Services.AddScoped<ITenantContext, TenantContext>();

var ssoAuthority = Environment.GetEnvironmentVariable("SSO_AUTHORITY") ?? "https://minisso.onrender.com";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.Authority = ssoAuthority;
    o.RequireHttpsMetadata = ssoAuthority.StartsWith("https");
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidIssuer = ssoAuthority,
        ValidateAudience = false, ValidateLifetime = true, NameClaimType = "name", RoleClaimType = "role"
    };
});
builder.Services.AddAuthorization();
builder.Services.AddFleetObs();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
    await Seeder.SeedAsync(scope.ServiceProvider.GetRequiredService<AppDbContext>());

app.UseFleetObs();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/whoami", (ClaimsPrincipal u) => Results.Ok(new
{
    app = "minihtc",
    sub = u.FindFirst("sub")?.Value, name = u.Identity?.Name ?? u.FindFirst("name")?.Value,
    email = u.FindFirst("email")?.Value, roles = u.FindAll("role").Select(c => c.Value)
})).RequireAuthorization();

app.Use(async (ctx, next) =>
{
    var key = ctx.Request.Headers["X-Api-Key"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(key)) ctx.Request.Cookies.TryGetValue(TenantContext.CookieName, out key);
    if (!string.IsNullOrWhiteSpace(key))
    {
        using var lookup = app.Services.CreateScope();
        var ldb = lookup.ServiceProvider.GetRequiredService<AppDbContext>();
        var org = await ldb.Orgs.FirstOrDefaultAsync(o => o.ApiKey == key);
        if (org != null) ctx.RequestServices.GetRequiredService<ITenantContext>().OrgId = org.Id;
    }
    await next();
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapGet("/healthz", () => "ok");

// ===== Màn "Quản lý Khu vực" (port 1:1 FrmArea) — CRUD Mst_Area =====
app.MapGet("/api/areas", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.Areas.Where(a => a.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(a => a.AreaCode.Contains(q) || a.AreaName.Contains(q));
    var items = await query.OrderBy(a => a.AreaCode)
        .Select(a => new { a.AreaCode, a.AreaName, a.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/areas", async (AreaDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.AreaCode) || string.IsNullOrWhiteSpace(dto.AreaName))
        return Results.BadRequest(new { error = "Cần AreaCode và AreaName." });
    var code = dto.AreaCode.Trim().ToUpperInvariant();
    var a = await db.Areas.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.AreaCode == code);
    if (a is null) { a = new Area { OrgId = t.OrgId, AreaCode = code, AreaName = dto.AreaName.Trim(), Status = dto.Status ?? "1" }; db.Areas.Add(a); }
    else { a.AreaName = dto.AreaName.Trim(); a.Status = dto.Status ?? a.Status; }   // btnApply: upsert
    await db.SaveChangesAsync();
    return Results.Ok(new { a.AreaCode, a.AreaName, a.Status });
}).RequireAuthorization();

app.MapDelete("/api/areas/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var a = await db.Areas.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.AreaCode == code);
    if (a is null) return Results.NotFound(new { code });
    db.Areas.Remove(a); await db.SaveChangesAsync();   // btnDelDb
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Master generic (port 1:1 loạt Frm masters code/name) =====
// Catalog: mỗi mục = 1 màn Frm gốc của 2010.HTC.
var MasterCatalog = new (string Cat, string Label)[]
{
    ("Bank", "Ngân hàng (FrmBank)"),
    ("Color", "Màu xe (FrmColor)"),
    ("DealerType", "Loại đại lý (FrmDealerType)"),
    ("CarCancelType", "Lý do hủy xe (FrmCarCancelType)"),
    ("ContractCancelType", "Lý do hủy hợp đồng (FrmContractCancelType)"),
    ("Certificate", "Chứng chỉ/Chứng nhận (FrmCertificate)"),
    ("InsCompany", "Công ty bảo hiểm (FrmMstInsCompany)"),
    ("InsType", "Loại bảo hiểm (FrmMstInsType)"),
    ("Maintenance", "Bảo dưỡng (FrmMaintenance)"),
    ("Department", "Phòng ban (FrmMngDepartment)"),
    ("District", "Quận/Huyện (FrmDistrict)"),
    ("Discount", "Chiết khấu (FrmDiscount)"),
    ("BusinessStatus", "Tình trạng KD (FrmBusinessStatus)"),
    ("Group", "Nhóm (FrmMngGroup)"),
    ("Position", "Chức vụ (FrmMst_Position)"),
    ("PaymentType", "Hình thức thanh toán (FrmPaymentType)"),
    ("PortType", "Loại cảng (FrmPortType)"),
    ("SalesGroupType", "Nhóm bán hàng (FrmSalesGroupType)"),
    ("SalesOrderType", "Loại đơn bán (FrmSalesOrderType)"),
    ("SalesType", "Loại bán hàng (FrmSalesType)"),
    ("StaffType", "Loại nhân viên (FrmStaffType)"),
    ("CarCancelReason", "Lý do hủy (FrmUpdateCar_Status)"),
    ("Model", "Model xe (FrmModel)"),
    ("Province", "Tỉnh/Thành (FrmProvince)"),
    ("Brand", "Nhãn hiệu (FrmBrand)"),
    ("Warehouse", "Kho (FrmWarehouse)"),
    ("Department2", "Bộ phận (FrmDepartment)"),
    ("AccountBank", "Tài khoản ngân hàng (FrmMstAccountBank)"),
    ("DealerBank", "Ngân hàng đại lý (FrmDealerBank)"),
    ("BusinessPlan", "Kế hoạch KD (FrmMngBusinessPlan)"),
    ("CarSpec", "Cấu hình xe (FrmCarSpec)"),
    ("CarOCN", "OCN xe (FrmCarOCN)"),
    ("Certificate2", "Chứng nhận (FrmCertificate)"),
    ("Marriage", "Tình trạng hôn nhân (FrmMst_Marriage)"),
    ("Qualification", "Trình độ (FrmMst_Qualification)"),
    ("PaymentTerm", "Điều khoản thanh toán (FrmMst_Dieu_Khoan_ThanhToan)"),
    ("DealerZone", "Vùng đại lý (FrmMst_DealerZone)"),
    ("ParamPDI", "Tham số PDI (FrmMst_ParamPDI)"),
    ("CarSpecInvoice", "Cấu hình HĐ (FrmCarSpecInvoice)"),
    ("InvoiceSetup", "Thiết lập hóa đơn (FrmMst_InvoiceSetup)"),
    ("CarAllocationArea", "Phân bổ xe theo vùng (FrmMst_CarAllocationByArea)"),
    ("SalesInvThreshold", "Ngưỡng tồn kho bán (FrmMstSalesInventoryThreshold)"),
    ("DealerInvThreshold", "Ngưỡng tồn ĐL (FrmMst_DealerInventoryThreshold)"),
    ("TiLeDatHang", "Tỉ lệ đặt hàng KH (FrmMstTiLeDatHangKeHoach)"),
    ("Nation", "Quốc gia (FrmNation)"),
    ("Ward", "Phường/Xã (FrmWard)"),
    ("Gender", "Giới tính (FrmMst_Gender)"),
    ("Religion", "Tôn giáo (FrmMst_Religion)"),
    ("CarStatus", "Trạng thái xe (FrmMst_CarStatus)"),
    ("ContractType", "Loại hợp đồng (FrmMst_ContractType)"),
    ("PromotionType", "Loại khuyến mãi (FrmMst_PromotionType)"),
    ("DocType", "Loại tài liệu (FrmMst_DocType)"),
    ("ExpenseType", "Loại chi phí (FrmMst_QuanLyLoaiChiPhi)"),
    ("Training", "Khóa đào tạo (FrmMst_TrainingMng)"),
    ("SalesManCert", "Chứng chỉ NVBH (FrmMst_SalesManCertificateMng)"),
    ("StorageGlobal", "Kho ảo (FrmMst_StorageGlobal)"),
    ("TransportDelay", "Hạn mức trễ vận tải (FrmMst_QuanLyHanMucDoTreVanTai)"),
    ("ProductionYear", "Năm SX VIN (FrmMst_VINProductionYear_Actual)"),
    ("StorageRate", "Định mức lưu kho (FrmMst_StorageRate)"),
    ("DevicePrice", "Giá thiết bị (FrmMst_DevicePrice_Spec)"),
    // ---- TCMotor (2021.1) service/bảo hành ----
    ("Supplier", "Nhà cung cấp (FrmMstSupplierCreate) [TCMotor]"),
    ("WarrantyPeriod", "Thời hạn bảo hành (FrmMngMst_WarrantyPeriod) [TCMotor]"),
    ("MaintenanceLevel", "Cấp bảo dưỡng (FrmMstMaintenanceLevelMng) [TCMotor]"),
    ("ExtraWork", "Công việc phát sinh (FrmMstExtraWorkMng) [TCMotor]"),
    ("ExtraParts", "Phụ tùng phát sinh (FrmMstExtraPartsMng) [TCMotor]"),
    ("WarrantyImageType", "Loại ảnh bảo hành (FrmMstWarrantyImageTypeMng) [TCMotor]"),
    ("ErrorCode", "Mã lỗi chẩn đoán (FrmMstComplaint...ErrorCodeMng) [TCMotor]"),
    ("CarModelStd", "Model chuẩn (FrmMstCarModelStd) [TCMotor]"),
    ("WarrantyExtItem", "Hạng mục gia hạn BH (FrmMstWarrantyExtensionItemMng) [TCMotor]"),
    ("BOM", "Định mức vật tư BOM (FrmMstBOMMng) [TCMotor]"),
};

app.MapGet("/api/master-categories", () => Results.Ok(new
{
    count = MasterCatalog.Length,
    items = MasterCatalog.Select(c => new { cat = c.Cat, label = c.Label })
})).RequireAuthorization();

app.MapGet("/api/master/{cat}", async (string cat, AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.Masters.Where(m => m.OrgId == t.OrgId && m.Category == cat);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(m => m.Code.Contains(q) || m.Name.Contains(q));
    var items = await query.OrderBy(m => m.Code).Select(m => new { m.Code, m.Name, m.Status }).ToListAsync();
    return Results.Ok(new { category = cat, count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/master/{cat}", async (string cat, MasterDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Code) || string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest(new { error = "Cần Code và Name." });
    var code = dto.Code.Trim().ToUpperInvariant();
    var m = await db.Masters.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Category == cat && x.Code == code);
    if (m is null) { m = new MasterItem { OrgId = t.OrgId, Category = cat, Code = code, Name = dto.Name.Trim(), Status = dto.Status ?? "1" }; db.Masters.Add(m); }
    else { m.Name = dto.Name.Trim(); m.Status = dto.Status ?? m.Status; }
    await db.SaveChangesAsync();
    return Results.Ok(new { category = cat, m.Code, m.Name, m.Status });
}).RequireAuthorization();

app.MapDelete("/api/master/{cat}/{code}", async (string cat, string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var m = await db.Masters.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Category == cat && x.Code == code);
    if (m is null) return Results.NotFound(new { cat, code });
    db.Masters.Remove(m); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Đại lý (Mst_Dealer) — port 1:1 FrmDealer =====
app.MapGet("/api/dealers", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.Dealers.Where(d => d.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(d => d.DealerCode.Contains(q) || d.DealerName.Contains(q));
    var items = await query.OrderBy(d => d.DealerCode).Select(d => new
    { d.DealerCode, d.DealerName, d.BUCode, d.ProvinceCode, d.Address, d.Phone, d.Fax, d.Email, d.TaxCode, d.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/dealers", async (DealerDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.DealerName))
        return Results.BadRequest(new { error = "Cần DealerCode và DealerName." });
    var code = dto.DealerCode.Trim().ToUpperInvariant();
    var d = await db.Dealers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.DealerCode == code);
    if (d is null) { d = new Dealer { OrgId = t.OrgId, DealerCode = code }; db.Dealers.Add(d); }
    d.DealerName = dto.DealerName.Trim(); d.BUCode = dto.BUCode; d.ProvinceCode = dto.ProvinceCode;
    d.Address = dto.Address; d.Phone = dto.Phone; d.Fax = dto.Fax; d.Email = dto.Email; d.TaxCode = dto.TaxCode;
    d.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { d.DealerCode, d.DealerName, d.Status });
}).RequireAuthorization();

app.MapDelete("/api/dealers/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var d = await db.Dealers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.DealerCode == code);
    if (d is null) return Results.NotFound(new { code });
    db.Dealers.Remove(d); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Bảng giá xe (Mst_CarPrice) — port 1:1 FrmCarPrice =====
app.MapGet("/api/carprices", async (AppDbContext db, ITenantContext t, string? model) =>
{
    var query = db.CarPrices.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(model)) query = query.Where(c => c.ModelCode.Contains(model));
    var items = await query.OrderBy(c => c.ModelCode).Select(c => new
    { c.Id, c.ModelCode, c.SpecCode, c.ColorCode, c.Price, c.Vat, priceVat = c.Price * (1 + c.Vat / 100), c.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/carprices", async (CarPriceDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.ModelCode)) return Results.BadRequest(new { error = "Cần ModelCode." });
    if (dto.Price < 0) return Results.BadRequest(new { error = "Price không hợp lệ." });
    var model = dto.ModelCode.Trim().ToUpperInvariant();
    var spec = dto.SpecCode?.Trim().ToUpperInvariant() ?? "";
    var color = dto.ColorCode?.Trim().ToUpperInvariant() ?? "";
    var c = await db.CarPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ModelCode == model && (x.SpecCode ?? "") == spec && (x.ColorCode ?? "") == color);
    if (c is null) { c = new CarPrice { OrgId = t.OrgId, ModelCode = model, SpecCode = spec, ColorCode = color }; db.CarPrices.Add(c); }
    c.Price = dto.Price; c.Vat = dto.Vat ?? 10; c.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { c.ModelCode, c.SpecCode, c.ColorCode, c.Price, c.Vat });
}).RequireAuthorization();

app.MapDelete("/api/carprices/{id:long}", async (long id, AppDbContext db, ITenantContext t) =>
{
    var c = await db.CarPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Id == id);
    if (c is null) return Results.NotFound(new { id });
    db.CarPrices.Remove(c); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = id });
}).RequireAuthorization();

// ===== Khách hàng (Mst_Customer) — port 1:1 FrmCustomerBase =====
app.MapGet("/api/customers", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.Customers.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(c => c.CustomerCode.Contains(q) || c.CustomerName.Contains(q) || (c.Phone ?? "").Contains(q));
    var items = await query.OrderBy(c => c.CustomerCode).Take(500).Select(c => new
    { c.CustomerCode, c.CustomerName, c.Phone, c.IdCard, c.TaxCode, c.Address, c.Email, c.ProvinceCode, c.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/customers", async (CustomerDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.CustomerName)) return Results.BadRequest(new { error = "Cần CustomerName." });
    var code = string.IsNullOrWhiteSpace(dto.CustomerCode) ? "KH" + DateTime.Now.ToString("yyMMddHHmmss") : dto.CustomerCode.Trim().ToUpperInvariant();
    var c = await db.Customers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CustomerCode == code);
    if (c is null) { c = new Customer { OrgId = t.OrgId, CustomerCode = code }; db.Customers.Add(c); }
    c.CustomerName = dto.CustomerName.Trim(); c.Phone = dto.Phone; c.IdCard = dto.IdCard; c.TaxCode = dto.TaxCode;
    c.Address = dto.Address; c.Email = dto.Email; c.ProvinceCode = dto.ProvinceCode; c.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { c.CustomerCode, c.CustomerName, c.Status });
}).RequireAuthorization();

app.MapDelete("/api/customers/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var c = await db.Customers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CustomerCode == code);
    if (c is null) return Results.NotFound(new { code });
    db.Customers.Remove(c); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Nhân viên bán hàng (Mst_SalesMan) — port 1:1 FrmCreateSalesMan =====
app.MapGet("/api/salesmen", async (AppDbContext db, ITenantContext t, string? q, string? dealer) =>
{
    var query = db.SalesMen.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(s => s.SalesManCode.Contains(q) || s.SalesManName.Contains(q));
    if (!string.IsNullOrWhiteSpace(dealer)) query = query.Where(s => s.DealerCode == dealer);
    var items = await query.OrderBy(s => s.SalesManCode).Take(500).Select(s => new
    { s.SalesManCode, s.SalesManName, s.DealerCode, s.DepartmentCode, s.Phone, s.Email, s.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/salesmen", async (SalesManDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SalesManName)) return Results.BadRequest(new { error = "Cần SalesManName." });
    var code = string.IsNullOrWhiteSpace(dto.SalesManCode) ? "NV" + DateTime.Now.ToString("yyMMddHHmmss") : dto.SalesManCode.Trim().ToUpperInvariant();
    var s = await db.SalesMen.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SalesManCode == code);
    if (s is null) { s = new SalesMan { OrgId = t.OrgId, SalesManCode = code }; db.SalesMen.Add(s); }
    s.SalesManName = dto.SalesManName.Trim(); s.DealerCode = dto.DealerCode; s.DepartmentCode = dto.DepartmentCode;
    s.Phone = dto.Phone; s.Email = dto.Email; s.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { s.SalesManCode, s.SalesManName, s.Status });
}).RequireAuthorization();

app.MapDelete("/api/salesmen/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var s = await db.SalesMen.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SalesManCode == code);
    if (s is null) return Results.NotFound(new { code });
    db.SalesMen.Remove(s); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Báo cáo đại lý (port 1:1 kiểu FrmBC*/Rpt — read-only + lọc + tổng hợp) =====
app.MapGet("/api/reports/dealers", async (AppDbContext db, ITenantContext t, string? province, string? status) =>
{
    var q = db.Dealers.Where(d => d.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(province)) q = q.Where(d => d.ProvinceCode == province);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(d => d.Status == status);
    var rows = await q.OrderBy(d => d.ProvinceCode).ThenBy(d => d.DealerCode)
        .Select(d => new { d.DealerCode, d.DealerName, d.ProvinceCode, d.Phone, d.Status }).ToListAsync();
    var byProvince = rows.GroupBy(r => r.ProvinceCode ?? "(chưa có)")
        .Select(g => new { province = g.Key, count = g.Count() }).OrderByDescending(x => x.count).ToList();
    return Results.Ok(new
    {
        total = rows.Count,
        active = rows.Count(r => r.Status == "1"),
        inactive = rows.Count(r => r.Status != "1"),
        byProvince,
        rows
    });
}).RequireAuthorization();

// ===== PDI - Kiểm tra trước giao xe (port 1:1 FrmMngDlr_PDIRequest) =====
app.MapGet("/api/pdi", async (AppDbContext db, ITenantContext t, string? status, string? vin) =>
{
    var q = db.PdiRequests.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.Status == status);
    if (!string.IsNullOrWhiteSpace(vin)) { var v = vin.Trim().ToUpperInvariant(); q = q.Where(p => p.Vin == v); }
    var items = await q.OrderByDescending(p => p.Id).Take(500).Select(p => new
    { p.Code, p.Vin, p.DealerCode, p.Status, p.Inspector, p.Result, p.CreatedAt, p.InspectedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/pdi", async (PdiDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var code = "PDI" + DateTime.Now.ToString("yyMMddHHmmss");
    var p = new PdiRequest { OrgId = t.OrgId, Code = code, Vin = dto.Vin.Trim().ToUpperInvariant(), DealerCode = dto.DealerCode ?? "", Status = "Requested" };
    db.PdiRequests.Add(p); await db.SaveChangesAsync();
    return Results.Ok(new { p.Code, p.Vin, status = p.Status });
}).RequireAuthorization();

app.MapPost("/api/pdi/{code}/{action}", async (string code, string action, PdiResultDto? dto, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("start" or "pass" or "fail")) return Results.BadRequest(new { error = "action = start|pass|fail" });
    code = code.Trim().ToUpperInvariant();
    var p = await db.PdiRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Code == code);
    if (p is null) return Results.NotFound(new { code });
    if (action == "start") { if (p.Status != "Requested") return Results.BadRequest(new { error = "Sai trạng thái." }); p.Status = "Inspecting"; p.Inspector = dto?.Inspector; }
    else { if (p.Status != "Inspecting") return Results.BadRequest(new { error = "Chưa bắt đầu kiểm tra." }); p.Status = action == "pass" ? "Passed" : "Failed"; p.Result = dto?.Result; p.InspectedAt = DateTime.Now; }
    await db.SaveChangesAsync();
    return Results.Ok(new { p.Code, p.Vin, status = p.Status, p.Inspector, p.Result });
}).RequireAuthorization();

// ===== Thu hồi xe (port 1:1 FrmMngCarRetrieve) =====
app.MapGet("/api/retrieves", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.CarRetrieves.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(r => r.Status == status);
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    { r.Code, r.Vin, r.DealerCode, r.Reason, r.Status, r.CreatedAt, r.ApprovedAt, r.RetrievedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/retrieves", async (RetrieveDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var code = "TH" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new CarRetrieve { OrgId = t.OrgId, Code = code, Vin = dto.Vin.Trim().ToUpperInvariant(), DealerCode = dto.DealerCode ?? "", Reason = dto.Reason, Status = "Requested" };
    db.CarRetrieves.Add(r); await db.SaveChangesAsync();
    return Results.Ok(new { r.Code, r.Vin, status = r.Status });
}).RequireAuthorization();

app.MapPost("/api/retrieves/{code}/{action}", async (string code, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject" or "retrieve")) return Results.BadRequest(new { error = "action = approve|reject|retrieve" });
    code = code.Trim().ToUpperInvariant();
    var r = await db.CarRetrieves.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Code == code);
    if (r is null) return Results.NotFound(new { code });
    var now = DateTime.Now;
    if (action == "approve") { if (r.Status != "Requested") return Results.BadRequest(new { error = "Sai trạng thái." }); r.Status = "Approved"; r.ApprovedAt = now; }
    else if (action == "reject") { if (r.Status != "Requested") return Results.BadRequest(new { error = "Sai trạng thái." }); r.Status = "Rejected"; r.ApprovedAt = now; }
    else { if (r.Status != "Approved") return Results.BadRequest(new { error = "Chưa duyệt." }); r.Status = "Retrieved"; r.RetrievedAt = now; }
    await db.SaveChangesAsync();
    return Results.Ok(new { r.Code, r.Vin, status = r.Status });
}).RequireAuthorization();

// ===== Hủy xe (port 1:1 FrmMngCarCancel) =====
app.MapGet("/api/cancels", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.CarCancels.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(c => c.Status == status);
    var items = await q.OrderByDescending(c => c.Id).Take(500).Select(c => new
    { c.Code, c.Vin, c.CancelTypeCode, c.Reason, c.Status, c.CreatedAt, c.ApprovedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/cancels", async (CancelDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var code = "HX" + DateTime.Now.ToString("yyMMddHHmmss");
    var c = new CarCancel { OrgId = t.OrgId, Code = code, Vin = dto.Vin.Trim().ToUpperInvariant(), CancelTypeCode = dto.CancelTypeCode, Reason = dto.Reason, Status = "Requested" };
    db.CarCancels.Add(c); await db.SaveChangesAsync();
    return Results.Ok(new { c.Code, c.Vin, status = c.Status });
}).RequireAuthorization();

app.MapPost("/api/cancels/{code}/{action}", async (string code, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject")) return Results.BadRequest(new { error = "action = approve|reject" });
    code = code.Trim().ToUpperInvariant();
    var c = await db.CarCancels.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Code == code);
    if (c is null) return Results.NotFound(new { code });
    if (c.Status != "Requested") return Results.BadRequest(new { error = "Đã xử lý." });
    c.Status = action == "approve" ? "Approved" : "Rejected"; c.ApprovedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { c.Code, c.Vin, status = c.Status });
}).RequireAuthorization();

// ===== Cấu hình hệ thống (port 1:1 FrmMngConfig*/Setup) =====
app.MapGet("/api/configs", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.Configs.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(c => c.ConfigKey.Contains(q));
    var items = await query.OrderBy(c => c.ConfigKey).Select(c => new { c.ConfigKey, c.ConfigValue, c.Description, c.UpdatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/configs", async (ConfigDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.ConfigKey)) return Results.BadRequest(new { error = "Cần ConfigKey." });
    var key = dto.ConfigKey.Trim();
    var c = await db.Configs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ConfigKey == key);
    if (c is null) { c = new SysConfig { OrgId = t.OrgId, ConfigKey = key }; db.Configs.Add(c); }
    c.ConfigValue = dto.ConfigValue ?? ""; c.Description = dto.Description; c.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { c.ConfigKey, c.ConfigValue });
}).RequireAuthorization();

app.MapDelete("/api/configs/{key}", async (string key, AppDbContext db, ITenantContext t) =>
{
    var c = await db.Configs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ConfigKey == key);
    if (c is null) return Results.NotFound(new { key });
    db.Configs.Remove(c); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = key });
}).RequireAuthorization();

// ===== Kế hoạch/chỉ tiêu KD (port 1:1 FrmMngBusinessPlan) =====
app.MapGet("/api/plans", async (AppDbContext db, ITenantContext t, string? month, string? dealer) =>
{
    var q = db.BusinessPlans.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(month)) q = q.Where(p => p.Month == month);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(p => p.DealerCode == dealer);
    var rows = await q.OrderBy(p => p.Month).ThenBy(p => p.DealerCode).Take(500).Select(p => new
    { p.DealerCode, p.ModelCode, p.Month, p.TargetQty, p.ActualQty, achieve = p.TargetQty == 0 ? 0 : Math.Round(p.ActualQty * 100.0 / p.TargetQty, 1) }).ToListAsync();
    return Results.Ok(new { count = rows.Count, totalTarget = rows.Sum(r => r.TargetQty), totalActual = rows.Sum(r => r.ActualQty), rows });
}).RequireAuthorization();

app.MapPost("/api/plans", async (PlanDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.ModelCode) || string.IsNullOrWhiteSpace(dto.Month))
        return Results.BadRequest(new { error = "Cần DealerCode, ModelCode, Month (YYYYMM)." });
    var dealer = dto.DealerCode.Trim().ToUpperInvariant(); var model = dto.ModelCode.Trim().ToUpperInvariant(); var month = dto.Month.Trim();
    var p = await db.BusinessPlans.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.DealerCode == dealer && x.ModelCode == model && x.Month == month);
    if (p is null) { p = new BusinessPlan { OrgId = t.OrgId, DealerCode = dealer, ModelCode = model, Month = month }; db.BusinessPlans.Add(p); }
    p.TargetQty = dto.TargetQty; if (dto.ActualQty.HasValue) p.ActualQty = dto.ActualQty.Value; p.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.DealerCode, p.ModelCode, p.Month, p.TargetQty, p.ActualQty });
}).RequireAuthorization();

// ===== Lái thử xe (port 1:1 FrmMstCarDriverTest — TCMotor) =====
app.MapGet("/api/testdrives", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.TestDrives.Where(x => x.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(x => x.Status == status);
    var items = await q.OrderByDescending(x => x.Id).Take(500).Select(x => new
    { x.Code, x.CustomerName, x.Phone, x.ModelCode, x.DealerCode, x.ScheduledAt, x.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/testdrives", async (TestDriveDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.CustomerName) || string.IsNullOrWhiteSpace(dto.ModelCode))
        return Results.BadRequest(new { error = "Cần CustomerName và ModelCode." });
    if (dto.ScheduledAt == default) return Results.BadRequest(new { error = "Cần ScheduledAt." });
    var code = "TD" + DateTime.Now.ToString("yyMMddHHmmss");
    var x = new TestDrive { OrgId = t.OrgId, Code = code, CustomerName = dto.CustomerName.Trim(), Phone = dto.Phone ?? "", ModelCode = dto.ModelCode.Trim().ToUpperInvariant(), DealerCode = dto.DealerCode, ScheduledAt = dto.ScheduledAt, Status = "Booked" };
    db.TestDrives.Add(x); await db.SaveChangesAsync();
    return Results.Ok(new { x.Code, x.CustomerName, x.ModelCode, status = x.Status });
}).RequireAuthorization();

app.MapPost("/api/testdrives/{code}/{action}", async (string code, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("done" or "cancel")) return Results.BadRequest(new { error = "action = done|cancel" });
    code = code.Trim().ToUpperInvariant();
    var x = await db.TestDrives.FirstOrDefaultAsync(y => y.OrgId == t.OrgId && y.Code == code);
    if (x is null) return Results.NotFound(new { code });
    if (x.Status != "Booked") return Results.BadRequest(new { error = "Đã xử lý." });
    x.Status = action == "done" ? "Done" : "Cancelled";
    await db.SaveChangesAsync();
    return Results.Ok(new { x.Code, status = x.Status });
}).RequireAuthorization();

// ===== Yêu cầu bảo hành dịch vụ TCMotor (port 1:1 Warranty Claim) =====
app.MapGet("/api/wclaims", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.WarrantyClaims.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(c => c.Status == status);
    var items = await q.OrderByDescending(c => c.Id).Take(500).Select(c => new
    { c.ClaimNo, c.Vin, c.DealerCode, c.ErrorCode, c.PartsCost, c.LaborCost, total = c.PartsCost + c.LaborCost, c.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, approvedValue = items.Where(i => i.Status is "Approved" or "Paid").Sum(i => i.total), items });
}).RequireAuthorization();

app.MapPost("/api/wclaims", async (WClaimDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var no = "WC" + DateTime.Now.ToString("yyMMddHHmmss");
    var c = new WarrantyClaimTC { OrgId = t.OrgId, ClaimNo = no, Vin = dto.Vin.Trim().ToUpperInvariant(), DealerCode = dto.DealerCode ?? "", ErrorCode = dto.ErrorCode, PartsCost = dto.PartsCost, LaborCost = dto.LaborCost, Status = "Submitted" };
    db.WarrantyClaims.Add(c); await db.SaveChangesAsync();
    return Results.Ok(new { c.ClaimNo, c.Vin, total = c.PartsCost + c.LaborCost, status = c.Status });
}).RequireAuthorization();

app.MapPost("/api/wclaims/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject" or "pay")) return Results.BadRequest(new { error = "action = approve|reject|pay" });
    no = no.Trim().ToUpperInvariant();
    var c = await db.WarrantyClaims.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ClaimNo == no);
    if (c is null) return Results.NotFound(new { no });
    if (action == "approve") { if (c.Status != "Submitted") return Results.BadRequest(new { error = "Sai trạng thái." }); c.Status = "Approved"; c.DecidedAt = DateTime.Now; }
    else if (action == "reject") { if (c.Status != "Submitted") return Results.BadRequest(new { error = "Sai trạng thái." }); c.Status = "Rejected"; c.DecidedAt = DateTime.Now; }
    else { if (c.Status != "Approved") return Results.BadRequest(new { error = "Chưa duyệt." }); c.Status = "Paid"; }
    await db.SaveChangesAsync();
    return Results.Ok(new { c.ClaimNo, status = c.Status });
}).RequireAuthorization();

// ===== Đơn đặt hàng NCC (port 1:1 Supplier PO — TCMotor) =====
app.MapGet("/api/pos", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.SupplierPOs.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.Status == status);
    var items = await q.OrderByDescending(p => p.Id).Take(500).Select(p => new
    { p.PoNo, p.SupplierCode, p.Note, p.Total, p.Status, p.CreatedAt, p.SentAt, p.ReceivedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/pos", async (PODto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SupplierCode)) return Results.BadRequest(new { error = "Cần SupplierCode." });
    var no = "PO" + DateTime.Now.ToString("yyMMddHHmmss");
    var p = new SupplierPO { OrgId = t.OrgId, PoNo = no, SupplierCode = dto.SupplierCode.Trim().ToUpperInvariant(), Note = dto.Note, Total = dto.Total, Status = "Draft" };
    db.SupplierPOs.Add(p); await db.SaveChangesAsync();
    return Results.Ok(new { p.PoNo, p.SupplierCode, p.Total, status = p.Status });
}).RequireAuthorization();

app.MapPost("/api/pos/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("send" or "receive" or "cancel")) return Results.BadRequest(new { error = "action = send|receive|cancel" });
    no = no.Trim().ToUpperInvariant();
    var p = await db.SupplierPOs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PoNo == no);
    if (p is null) return Results.NotFound(new { no });
    var now = DateTime.Now;
    if (action == "send") { if (p.Status != "Draft") return Results.BadRequest(new { error = "Sai trạng thái." }); p.Status = "Sent"; p.SentAt = now; }
    else if (action == "receive") { if (p.Status != "Sent") return Results.BadRequest(new { error = "Chưa gửi." }); p.Status = "Received"; p.ReceivedAt = now; }
    else { if (p.Status == "Received") return Results.BadRequest(new { error = "Đã nhận." }); p.Status = "Cancelled"; }
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PoNo, status = p.Status });
}).RequireAuthorization();

app.MapPost("/api/orgs/register", async (RegisterOrgDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name)) return Results.BadRequest(new { error = "Cần Name." });
    var org = new Org { Name = dto.Name.Trim(), ApiKey = "htc_" + Guid.NewGuid().ToString("N") };
    db.Orgs.Add(org); await db.SaveChangesAsync();
    return Results.Ok(new { orgId = org.Id, apiKey = org.ApiKey });
});

app.Run();

record AreaDto(string AreaCode, string AreaName, string? Status);
record MasterDto(string Code, string Name, string? Status);
record DealerDto(string DealerCode, string DealerName, string? BUCode, string? ProvinceCode, string? Address, string? Phone, string? Fax, string? Email, string? TaxCode, string? Status);
record CarPriceDto(string ModelCode, string? SpecCode, string? ColorCode, decimal Price, decimal? Vat, string? Status);
record CustomerDto(string? CustomerCode, string CustomerName, string? Phone, string? IdCard, string? TaxCode, string? Address, string? Email, string? ProvinceCode, string? Status);
record SalesManDto(string? SalesManCode, string SalesManName, string? DealerCode, string? DepartmentCode, string? Phone, string? Email, string? Status);
record PdiDto(string Vin, string? DealerCode);
record PdiResultDto(string? Inspector, string? Result);
record RetrieveDto(string Vin, string? DealerCode, string? Reason);
record CancelDto(string Vin, string? CancelTypeCode, string? Reason);
record ConfigDto(string ConfigKey, string? ConfigValue, string? Description);
record PlanDto(string DealerCode, string ModelCode, string Month, int TargetQty, int? ActualQty);
record TestDriveDto(string CustomerName, string? Phone, string ModelCode, string? DealerCode, DateTime ScheduledAt);
record WClaimDto(string Vin, string? DealerCode, string? ErrorCode, decimal PartsCost, decimal LaborCost);
record PODto(string SupplierCode, string? Note, decimal Total);
record RegisterOrgDto(string Name);
