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
    ("WorkGroup", "Nhóm công việc (FrmMst_GroupRepair) [TCMotor]"),
    ("LaborRate", "Đơn giá công (FrmMst_LaborRate) [TCMotor]"),
    ("SymptomCode", "Mã hiện tượng (FrmMst_Symptom) [TCMotor]"),
    ("CauseCode", "Mã nguyên nhân (FrmMst_Cause) [TCMotor]"),
    ("RepairType", "Loại sửa chữa (FrmMst_RepairType) [TCMotor]"),
    ("ServiceType", "Loại dịch vụ (FrmMst_ServiceType) [TCMotor]"),
    ("Campaign", "Chiến dịch (FrmMst_Campaign) [TCMotor]"),
    ("VOCType", "Loại VOC/khiếu nại (FrmMst_VOCType) [TCMotor]"),
    ("SkillLevel", "Bậc thợ (FrmMst_SkillLevel) [TCMotor]"),
    ("Bay", "Khoang sửa chữa (FrmMst_Bay) [TCMotor]"),
    ("ToolMaster", "Dụng cụ (FrmMst_Tool) [TCMotor]"),
    ("PartCategory", "Nhóm phụ tùng (FrmMst_PartCategory) [TCMotor]"),
    ("ClaimRejectReason", "Lý do từ chối BH (FrmMst_ClaimRejectReason) [TCMotor]"),
    ("MaintenancePackage", "Gói bảo dưỡng (FrmMst_MaintenancePackage) [TCMotor]"),
    ("Currency", "Tiền tệ (FrmMst_Currency)"),
    // ---- Bổ sung 2010.HTC + TCMotor ----
    ("PriceType", "Loại giá (FrmMst_PriceType)"),
    ("DeliveryMethod", "Phương thức giao (FrmMst_DeliveryMethod)"),
    ("TransportRoute", "Tuyến vận tải (FrmMst_TransportRoute)"),
    ("PortMaster", "Cảng (FrmMst_Port)"),
    ("VesselMaster", "Tàu vận chuyển (FrmMst_Vessel)"),
    ("PackingType", "Loại đóng gói (FrmMst_PackingType)"),
    ("TaxRate", "Thuế suất (FrmMst_TaxRate)"),
    ("FeeType", "Loại phí (FrmMst_FeeType)"),
    ("AccountType", "Loại tài khoản (FrmMst_AccountType)"),
    ("CostCenter", "Trung tâm chi phí (FrmMst_CostCenter)"),
    ("Project", "Dự án (FrmMst_Project)"),
    ("Budget", "Ngân sách (FrmMst_Budget)"),
    ("ApprovalLevel", "Cấp phê duyệt (FrmMst_ApprovalLevel)"),
    ("Holiday", "Ngày lễ (FrmMst_Holiday)"),
    ("Shift", "Ca làm việc (FrmMst_Shift)"),
    ("LeaveType", "Loại nghỉ phép (FrmMst_LeaveType)"),
    ("NotifyTemplate", "Mẫu thông báo (FrmMst_NotifyTemplate)"),
    ("PrintTemplate", "Mẫu in (FrmMst_PrintTemplate)"),
    ("EmailTemplate", "Mẫu email (FrmMst_EmailTemplate)"),
    ("SmsTemplate", "Mẫu SMS (FrmMst_SmsTemplate)"),
    ("Menu", "Menu chức năng (FrmMst_Menu)"),
    ("Role2", "Nhóm quyền (FrmMst_Role)"),
    ("ReportType", "Loại báo cáo (FrmMst_ReportType)"),
    ("DashboardWidget", "Widget dashboard (FrmMst_DashboardWidget)"),
    ("Language", "Ngôn ngữ (FrmMst_Language)"),
    ("TimeZoneMaster", "Múi giờ (FrmMst_TimeZone)"),
    ("NumberSeries", "Dải số chứng từ (FrmMst_NumberSeries)"),
    ("WorkflowStep", "Bước quy trình (FrmMst_WorkflowStep)"),
    ("ReasonCode", "Mã lý do chung (FrmMst_ReasonCode)"),
    ("UomMaster", "Đơn vị tính (FrmMst_UOM)"),
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

// ===== BOM định mức bảo dưỡng (header-detail, port 1:1 FrmMstBOMMng — TCMotor) =====
app.MapGet("/api/boms", async (AppDbContext db, ITenantContext t, string? model) =>
{
    var q = db.Boms.Where(b => b.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(model)) q = q.Where(b => b.ModelCode.Contains(model));
    var items = await q.OrderBy(b => b.BomCode).Select(b => new
    { b.BomCode, b.ModelCode, b.MaintLevel, b.Status, lines = db.BomLines.Count(l => l.OrgId == t.OrgId && l.BomId == b.Id) }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/boms", async (BomDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.BomCode) || string.IsNullOrWhiteSpace(dto.ModelCode))
        return Results.BadRequest(new { error = "Cần BomCode và ModelCode." });
    var code = dto.BomCode.Trim().ToUpperInvariant();
    var b = await db.Boms.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.BomCode == code);
    if (b is null) { b = new Bom { OrgId = t.OrgId, BomCode = code }; db.Boms.Add(b); }
    b.ModelCode = dto.ModelCode.Trim().ToUpperInvariant(); b.MaintLevel = dto.MaintLevel; b.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { b.BomCode, b.ModelCode, b.MaintLevel });
}).RequireAuthorization();

app.MapGet("/api/boms/{code}/lines", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var b = await db.Boms.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.BomCode == code);
    if (b is null) return Results.NotFound(new { code });
    var lines = await db.BomLines.Where(l => l.OrgId == t.OrgId && l.BomId == b.Id).Select(l => new { l.Id, l.PartSku, l.PartName, l.Qty }).ToListAsync();
    return Results.Ok(new { bom = b.BomCode, count = lines.Count, lines });
}).RequireAuthorization();

app.MapPost("/api/boms/{code}/lines", async (string code, BomLineDto dto, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var b = await db.Boms.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.BomCode == code);
    if (b is null) return Results.NotFound(new { code });
    if (string.IsNullOrWhiteSpace(dto.PartSku)) return Results.BadRequest(new { error = "Cần PartSku." });
    db.BomLines.Add(new BomLine { OrgId = t.OrgId, BomId = b.Id, PartSku = dto.PartSku.Trim().ToUpperInvariant(), PartName = dto.PartName, Qty = dto.Qty <= 0 ? 1 : dto.Qty });
    await db.SaveChangesAsync();
    return Results.Ok(new { bom = code, dto.PartSku, dto.Qty });
}).RequireAuthorization();

app.MapDelete("/api/boms/lines/{id:long}", async (long id, AppDbContext db, ITenantContext t) =>
{
    var l = await db.BomLines.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Id == id);
    if (l is null) return Results.NotFound(new { id });
    db.BomLines.Remove(l); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = id });
}).RequireAuthorization();

// ===== Gia hạn bảo hành (port 1:1 FrmMstWarrantyExtension — TCMotor) =====
app.MapGet("/api/wexts", async (AppDbContext db, ITenantContext t, string? status) =>
{
    var q = db.WarrantyExts.Where(w => w.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(w => w.Status == status);
    var items = await q.OrderByDescending(w => w.Id).Take(500).Select(w => new
    { w.Code, w.Vin, w.ItemCode, w.ExtraMonths, w.Fee, w.Status, w.ActivatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/wexts", async (WExtDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin) || dto.ExtraMonths <= 0)
        return Results.BadRequest(new { error = "Cần Vin và ExtraMonths > 0." });
    var code = "WE" + DateTime.Now.ToString("yyMMddHHmmss");
    var w = new WarrantyExtension { OrgId = t.OrgId, Code = code, Vin = dto.Vin.Trim().ToUpperInvariant(), ItemCode = dto.ItemCode, ExtraMonths = dto.ExtraMonths, Fee = dto.Fee, Status = "Requested" };
    db.WarrantyExts.Add(w); await db.SaveChangesAsync();
    return Results.Ok(new { w.Code, w.Vin, w.ExtraMonths, w.Fee, status = w.Status });
}).RequireAuthorization();

app.MapPost("/api/wexts/{code}/{action}", async (string code, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("pay" or "activate" or "cancel")) return Results.BadRequest(new { error = "action = pay|activate|cancel" });
    code = code.Trim().ToUpperInvariant();
    var w = await db.WarrantyExts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Code == code);
    if (w is null) return Results.NotFound(new { code });
    if (action == "pay") { if (w.Status != "Requested") return Results.BadRequest(new { error = "Sai trạng thái." }); w.Status = "Paid"; }
    else if (action == "activate") { if (w.Status != "Paid") return Results.BadRequest(new { error = "Chưa thanh toán." }); w.Status = "Activated"; w.ActivatedAt = DateTime.Now; }
    else { if (w.Status == "Activated") return Results.BadRequest(new { error = "Đã kích hoạt." }); w.Status = "Cancelled"; }
    await db.SaveChangesAsync();
    return Results.Ok(new { w.Code, status = w.Status });
}).RequireAuthorization();

// ===== Đề nghị thế chấp xe (RM_ReqMortgage — port 1:1 FrmNewRM_ReqMortgage/FrmMngRM_ReqMortgage) =====
// Header + lô xe. Pending(Mới tạo) → Approved(Đang thế chấp) → Finished(Đã giải chấp).
app.MapGet("/api/mortgages", async (AppDbContext db, ITenantContext t, string? status, string? bank) =>
{
    var q = db.MortgageRequests.Where(m => m.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(m => m.Status == status);
    if (!string.IsNullOrWhiteSpace(bank)) q = q.Where(m => m.BankCode == bank);
    var items = await q.OrderByDescending(m => m.Id).Take(500).Select(m => new
    {
        m.Id, m.ReqRMNo, m.BankCode, m.Status, m.CreatedAt, m.ApprovedAt, m.FinishedAt,
        cars = db.MortgageCars.Count(c => c.OrgId == t.OrgId && c.ReqId == m.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/mortgages", async (MortgageDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.BankCode)) return Results.BadRequest(new { error = "Cần BankCode (ngân hàng nhận thế chấp)." });
    var vins = (dto.Vins ?? new List<string>()).Select(v => (v ?? "").Trim().ToUpperInvariant()).Where(v => v.Length > 0).Distinct().ToList();
    if (vins.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    var reqNo = "RM" + DateTime.Now.ToString("yyMMddHHmmss");
    var m = new MortgageRequest { OrgId = t.OrgId, ReqRMNo = reqNo, BankCode = dto.BankCode.Trim().ToUpperInvariant(), Status = "Pending" };
    db.MortgageRequests.Add(m);
    await db.SaveChangesAsync();
    foreach (var v in vins)
        db.MortgageCars.Add(new MortgageCar { OrgId = t.OrgId, ReqId = m.Id, Vin = v, DtlStatus = "Pending" });
    await db.SaveChangesAsync();
    return Results.Ok(new { m.ReqRMNo, m.BankCode, status = m.Status, cars = vins.Count });
}).RequireAuthorization();

app.MapGet("/api/mortgages/{reqNo}/cars", async (string reqNo, AppDbContext db, ITenantContext t) =>
{
    reqNo = reqNo.Trim().ToUpperInvariant();
    var m = await db.MortgageRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqRMNo == reqNo);
    if (m is null) return Results.NotFound(new { reqNo });
    var cars = await db.MortgageCars.Where(c => c.OrgId == t.OrgId && c.ReqId == m.Id)
        .Select(c => new { c.Vin, c.ModelCode, c.EngineNo, c.DtlStatus }).ToListAsync();
    return Results.Ok(new { m.ReqRMNo, m.BankCode, m.Status, count = cars.Count, cars });
}).RequireAuthorization();

app.MapPost("/api/mortgages/{reqNo}/{action}", async (string reqNo, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "finish")) return Results.BadRequest(new { error = "action = approve|finish" });
    reqNo = reqNo.Trim().ToUpperInvariant();
    var m = await db.MortgageRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqRMNo == reqNo);
    if (m is null) return Results.NotFound(new { reqNo });
    if (action == "approve")
    {
        if (m.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt được đề nghị Mới tạo." });
        m.Status = "Approved"; m.ApprovedAt = DateTime.Now;
    }
    else // finish = giải chấp
    {
        if (m.Status != "Approved") return Results.BadRequest(new { error = "Chỉ giải chấp được đề nghị Đang thế chấp." });
        m.Status = "Finished"; m.FinishedAt = DateTime.Now;
    }
    var newDtl = action == "approve" ? "Approved" : "Finished";
    foreach (var c in await db.MortgageCars.Where(c => c.OrgId == t.OrgId && c.ReqId == m.Id).ToListAsync())
        c.DtlStatus = newDtl;
    await db.SaveChangesAsync();
    return Results.Ok(new { m.ReqRMNo, status = m.Status });
}).RequireAuthorization();

// ===== Phiếu chi / thanh toán (Pmt_Payment — port 1:1 FrmNewPM/FrmMngPM) =====
// Header + dòng chi. TotalAmount = Σ AmountCurrent; AmountTotal(dòng) = AmountAccum + AmountCurrent. Pending → Approved/Rejected.
app.MapGet("/api/pms", async (AppDbContext db, ITenantContext t, string? status, string? dealer) =>
{
    var q = db.PmtVouchers.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.Status == status);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(p => p.DealerCode == dealer);
    var items = await q.OrderByDescending(p => p.Id).Take(500).Select(p => new
    {
        p.PMNo, p.DealerCode, p.BankAccountSend, p.BankAccountReceive, p.TotalAmount, p.Status, p.CreatedAt, p.DecidedAt,
        lines = db.PmtLines.Count(l => l.OrgId == t.OrgId && l.VoucherId == p.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, total = items.Sum(x => x.TotalAmount), items });
}).RequireAuthorization();

app.MapPost("/api/pms", async (PmDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode)) return Results.BadRequest(new { error = "Cần DealerCode." });
    var lines = (dto.Lines ?? new List<PmLineDto>()).Where(l => !string.IsNullOrWhiteSpace(l.RefNo) && l.AmountCurrent > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng chi (RefNo + AmountCurrent > 0)." });
    var pmNo = "PM" + DateTime.Now.ToString("yyMMddHHmmss");
    var p = new PmtVoucher
    {
        OrgId = t.OrgId, PMNo = pmNo, DealerCode = dto.DealerCode.Trim().ToUpperInvariant(),
        BankAccountSend = dto.BankAccountSend, BankAccountReceive = dto.BankAccountReceive,
        TotalAmount = lines.Sum(l => l.AmountCurrent), Status = "Pending"
    };
    db.PmtVouchers.Add(p);
    await db.SaveChangesAsync();
    foreach (var l in lines)
        db.PmtLines.Add(new PmtLine { OrgId = t.OrgId, VoucherId = p.Id, RefNo = l.RefNo.Trim().ToUpperInvariant(), AmountAccum = l.AmountAccum, AmountCurrent = l.AmountCurrent });
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PMNo, p.DealerCode, total = p.TotalAmount, lines = lines.Count, status = p.Status });
}).RequireAuthorization();

app.MapGet("/api/pms/{pmNo}/lines", async (string pmNo, AppDbContext db, ITenantContext t) =>
{
    pmNo = pmNo.Trim().ToUpperInvariant();
    var p = await db.PmtVouchers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PMNo == pmNo);
    if (p is null) return Results.NotFound(new { pmNo });
    var lines = await db.PmtLines.Where(l => l.OrgId == t.OrgId && l.VoucherId == p.Id)
        .Select(l => new { l.RefNo, l.AmountAccum, l.AmountCurrent, amountTotal = l.AmountAccum + l.AmountCurrent }).ToListAsync();
    return Results.Ok(new { p.PMNo, p.DealerCode, p.TotalAmount, p.Status, count = lines.Count, lines });
}).RequireAuthorization();

app.MapPost("/api/pms/{pmNo}/{action}", async (string pmNo, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject")) return Results.BadRequest(new { error = "action = approve|reject" });
    pmNo = pmNo.Trim().ToUpperInvariant();
    var p = await db.PmtVouchers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PMNo == pmNo);
    if (p is null) return Results.NotFound(new { pmNo });
    if (p.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt/từ chối phiếu Chờ duyệt." });
    p.Status = action == "approve" ? "Approved" : "Rejected"; p.DecidedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PMNo, status = p.Status });
}).RequireAuthorization();

// ===== Bảo lãnh / LC ngân hàng (Guarantee — port 1:1 FrmNewGrt/FrmMngGrt + FrmEditGrtExpiredDate) =====
app.MapGet("/api/grts", async (AppDbContext db, ITenantContext t, string? status, string? type) =>
{
    var q = db.Guarantees.Where(g => g.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(g => g.Status == status);
    if (!string.IsNullOrWhiteSpace(type)) q = q.Where(g => g.GrtType == type);
    var now = DateTime.Now;
    var items = await q.OrderByDescending(g => g.Id).Take(500).Select(g => new
    {
        g.GrtNo, g.BankGrtNo, g.BankCode, g.GrtType, g.GrtValue, g.GrtDate, g.DateExpired, g.Status,
        expired = g.DateExpired != null && g.DateExpired < now
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, totalValue = items.Sum(x => x.GrtValue), items });
}).RequireAuthorization();

app.MapPost("/api/grts", async (GrtDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.BankCode)) return Results.BadRequest(new { error = "Cần BankCode (ngân hàng bảo lãnh)." });
    if (dto.GrtValue <= 0) return Results.BadRequest(new { error = "Giá trị bảo lãnh phải > 0." });
    var type = string.IsNullOrWhiteSpace(dto.GrtType) ? "BL" : dto.GrtType.Trim().ToUpperInvariant();
    if (type is not ("BL" or "LCTC" or "LCUP" or "EPLC")) return Results.BadRequest(new { error = "GrtType = BL|LCTC|LCUP|EPLC" });
    var grtNo = "GRT" + DateTime.Now.ToString("yyMMddHHmmss");
    var g = new Guarantee
    {
        OrgId = t.OrgId, GrtNo = grtNo, BankGrtNo = dto.BankGrtNo, BankCode = dto.BankCode.Trim().ToUpperInvariant(),
        GrtType = type, GrtValue = dto.GrtValue,
        GrtDate = dto.GrtDate ?? DateTime.Now, DateExpired = dto.DateExpired, Status = "Pending"
    };
    db.Guarantees.Add(g); await db.SaveChangesAsync();
    return Results.Ok(new { g.GrtNo, g.BankCode, g.GrtType, g.GrtValue, status = g.Status });
}).RequireAuthorization();

app.MapPost("/api/grts/{grtNo}/approve", async (string grtNo, AppDbContext db, ITenantContext t) =>
{
    grtNo = grtNo.Trim().ToUpperInvariant();
    var g = await db.Guarantees.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GrtNo == grtNo);
    if (g is null) return Results.NotFound(new { grtNo });
    if (g.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt bảo lãnh Mới tạo." });
    g.Status = "Approved"; g.ApprovedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GrtNo, status = g.Status });
}).RequireAuthorization();

// Sửa ngày hết hạn (FrmEditGrtExpiredDate / FrmEditGrtEndDate)
app.MapPost("/api/grts/{grtNo}/expiry", async (string grtNo, GrtExpiryDto dto, AppDbContext db, ITenantContext t) =>
{
    grtNo = grtNo.Trim().ToUpperInvariant();
    var g = await db.Guarantees.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GrtNo == grtNo);
    if (g is null) return Results.NotFound(new { grtNo });
    if (dto.DateExpired is null) return Results.BadRequest(new { error = "Cần DateExpired." });
    g.DateExpired = dto.DateExpired;
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GrtNo, g.DateExpired });
}).RequireAuthorization();

// ===== Danh sách hóa đơn xuất bán (InvoiceList — port 1:1 FrmNewInvoice/FrmMngInvoice) =====
app.MapGet("/api/invoicelists", async (AppDbContext db, ITenantContext t) =>
{
    var items = await db.InvoiceLists.Where(l => l.OrgId == t.OrgId).OrderByDescending(l => l.Id).Take(500)
        .Select(l => new { l.InvoiceListCode, l.CreatedDate, lines = db.InvoiceLines.Count(d => d.OrgId == t.OrgId && d.ListId == l.Id) }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/invoicelists", async (InvoiceListDto dto, AppDbContext db, ITenantContext t) =>
{
    var lines = (dto.Lines ?? new List<InvoiceLineDto>()).Where(l => !string.IsNullOrWhiteSpace(l.InvoiceNo) && !string.IsNullOrWhiteSpace(l.Vin)).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng (InvoiceNo + Vin)." });
    var code = "IVL" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new InvoiceList { OrgId = t.OrgId, InvoiceListCode = code, CreatedDate = DateTime.Now };
    db.InvoiceLists.Add(h); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.InvoiceLines.Add(new InvoiceLine { OrgId = t.OrgId, ListId = h.Id, CarId = l.CarId, DealerCode = l.DealerCode, InvoiceNo = l.InvoiceNo.Trim(), Vin = l.Vin.Trim().ToUpperInvariant(), InvoiceDate = l.InvoiceDate });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.InvoiceListCode, lines = lines.Count });
}).RequireAuthorization();

app.MapGet("/api/invoicelists/{code}/lines", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var h = await db.InvoiceLists.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.InvoiceListCode == code);
    if (h is null) return Results.NotFound(new { code });
    var lines = await db.InvoiceLines.Where(l => l.OrgId == t.OrgId && l.ListId == h.Id)
        .Select(l => new { l.CarId, l.DealerCode, l.InvoiceNo, l.Vin, l.InvoiceDate }).ToListAsync();
    return Results.Ok(new { h.InvoiceListCode, count = lines.Count, lines });
}).RequireAuthorization();

app.MapDelete("/api/invoicelists/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var h = await db.InvoiceLists.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.InvoiceListCode == code);
    if (h is null) return Results.NotFound(new { code });
    db.InvoiceLines.RemoveRange(db.InvoiceLines.Where(l => l.OrgId == t.OrgId && l.ListId == h.Id));
    db.InvoiceLists.Remove(h);
    await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Biên bản bàn giao theo hối phiếu NH (Car_BankBillMinutes — port 1:1 FrmTaoBBBG/FrmQuanLyBBBG) =====
app.MapGet("/api/bankbills", async (AppDbContext db, ITenantContext t, string? status, string? bank) =>
{
    var q = db.BankBillMinutes.Where(b => b.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(b => b.Status == status);
    if (!string.IsNullOrWhiteSpace(bank)) q = q.Where(b => b.BankCode == bank);
    var items = await q.OrderByDescending(b => b.Id).Take(500).Select(b => new
    {
        b.BankBillMnNo, b.BankCode, b.BankBillDate, b.BankBillReciveDate, b.Status, b.CreatedDateTime,
        cars = db.BankBillCars.Count(c => c.OrgId == t.OrgId && c.BillId == b.Id),
        claimTotal = db.BankBillCars.Where(c => c.OrgId == t.OrgId && c.BillId == b.Id).Sum(c => (decimal?)c.ClaimAmount) ?? 0
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/bankbills", async (BankBillDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.BankCode)) return Results.BadRequest(new { error = "Cần BankCode." });
    var cars = (dto.Cars ?? new List<BankBillCarDto>()).Where(c => !string.IsNullOrWhiteSpace(c.Vin)).ToList();
    if (cars.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    // check trùng VIN trong lô (port đúng "VIN bị trùng!" của FrmTaoBBBG)
    var dupe = cars.GroupBy(c => c.Vin.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"VIN {dupe.Key} bị trùng!" });
    var no = "BBBG" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new BankBillMinutes { OrgId = t.OrgId, BankBillMnNo = no, BankCode = dto.BankCode.Trim().ToUpperInvariant(), BankBillDate = dto.BankBillDate, Status = "Created" };
    db.BankBillMinutes.Add(h); await db.SaveChangesAsync();
    foreach (var c in cars)
        db.BankBillCars.Add(new BankBillCar { OrgId = t.OrgId, BillId = h.Id, Vin = c.Vin.Trim().ToUpperInvariant(), EngineNo = c.EngineNo, LCNo = c.LCNo, GuaranteeBankCode = c.GuaranteeBankCode, ClaimAmount = c.ClaimAmount });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.BankBillMnNo, h.BankCode, cars = cars.Count, status = h.Status });
}).RequireAuthorization();

app.MapGet("/api/bankbills/{no}/cars", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.BankBillMinutes.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.BankBillMnNo == no);
    if (h is null) return Results.NotFound(new { no });
    var cars = await db.BankBillCars.Where(c => c.OrgId == t.OrgId && c.BillId == h.Id)
        .Select(c => new { c.Vin, c.EngineNo, c.LCNo, c.GuaranteeBankCode, c.ClaimAmount }).ToListAsync();
    return Results.Ok(new { h.BankBillMnNo, h.Status, count = cars.Count, cars });
}).RequireAuthorization();

app.MapPost("/api/bankbills/{no}/receive", async (string no, BankBillReceiveDto dto, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.BankBillMinutes.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.BankBillMnNo == no);
    if (h is null) return Results.NotFound(new { no });
    if (h.Status != "Created") return Results.BadRequest(new { error = "Chỉ ghi nhận hối phiếu cho BBBG Mới tạo." });
    h.Status = "Received"; h.BankBillReciveDate = dto.BankBillReciveDate ?? DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { h.BankBillMnNo, status = h.Status, h.BankBillReciveDate });
}).RequireAuthorization();

// ===== Yêu cầu vận chuyển xe (TransportRequest — port 1:1 FrmNewTransportRequest/FrmMngTransportRequest, Phase2) =====
app.MapGet("/api/transreqs", async (AppDbContext db, ITenantContext t, string? status, string? transporter) =>
{
    var q = db.TransportRequests.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(r => r.Status == status);
    if (!string.IsNullOrWhiteSpace(transporter)) q = q.Where(r => r.TransporterCode == transporter);
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    {
        r.TranspReqNo, r.DealerCode, r.TransporterCode, r.TransContractNo, r.Status, r.CreatedAt, r.DecidedAt,
        cars = db.TransportReqCars.Count(c => c.OrgId == t.OrgId && c.ReqId == r.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/transreqs", async (TransReqDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.TransporterCode))
        return Results.BadRequest(new { error = "Cần DealerCode và TransporterCode." });
    var vins = (dto.Cars ?? new List<TransReqCarDto>()).Where(c => !string.IsNullOrWhiteSpace(c.Vin)).ToList();
    if (vins.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    var dupe = vins.GroupBy(c => c.Vin.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"VIN {dupe.Key} bị trùng!" });
    var no = "TR" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new TransportRequest
    {
        OrgId = t.OrgId, TranspReqNo = no, DealerCode = dto.DealerCode.Trim().ToUpperInvariant(),
        TransporterCode = dto.TransporterCode.Trim().ToUpperInvariant(), TransContractNo = dto.TransContractNo, Status = "Pending"
    };
    db.TransportRequests.Add(r); await db.SaveChangesAsync();
    foreach (var c in vins)
        db.TransportReqCars.Add(new TransportReqCar { OrgId = t.OrgId, ReqId = r.Id, Vin = c.Vin.Trim().ToUpperInvariant(), DoNo = c.DoNo, ColorCode = c.ColorCode, StorageCode = c.StorageCode });
    await db.SaveChangesAsync();
    return Results.Ok(new { r.TranspReqNo, r.DealerCode, r.TransporterCode, cars = vins.Count, status = r.Status });
}).RequireAuthorization();

app.MapGet("/api/transreqs/{no}/cars", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.TransportRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TranspReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    var cars = await db.TransportReqCars.Where(c => c.OrgId == t.OrgId && c.ReqId == r.Id)
        .Select(c => new { c.Vin, c.DoNo, c.ColorCode, c.StorageCode }).ToListAsync();
    return Results.Ok(new { r.TranspReqNo, r.Status, count = cars.Count, cars });
}).RequireAuthorization();

app.MapPost("/api/transreqs/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject")) return Results.BadRequest(new { error = "action = approve|reject" });
    no = no.Trim().ToUpperInvariant();
    var r = await db.TransportRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TranspReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt/từ chối yêu cầu Đang xử lý." });
    r.Status = action == "approve" ? "Approved" : "Rejected"; r.DecidedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.TranspReqNo, status = r.Status });
}).RequireAuthorization();

// ===== Phí vận chuyển theo tuyến (Mst_TranspFee — port 1:1 FrmNewTranspFee/FrmMngTranspFee, Phase2) =====
app.MapGet("/api/transpfees", async (AppDbContext db, ITenantContext t, string? transporter, string? model) =>
{
    var q = db.TranspFees.Where(f => f.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(transporter)) q = q.Where(f => f.TransporterCode == transporter);
    if (!string.IsNullOrWhiteSpace(model)) q = q.Where(f => f.ModelCode == model);
    var items = await q.OrderBy(f => f.ProvinceCodeFrom).ThenBy(f => f.ProvinceCodeTo).Take(1000).Select(f => new
    {
        f.ProvinceCodeFrom, f.ProvinceCodeTo, f.DistrictCodeFrom, f.DistrictCodeTo, f.TransporterCode, f.ModelCode, f.ValFee, f.ExpectedDays
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/transpfees", async (TranspFeeDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.ProvinceCodeFrom) || string.IsNullOrWhiteSpace(dto.ProvinceCodeTo)
        || string.IsNullOrWhiteSpace(dto.TransporterCode) || string.IsNullOrWhiteSpace(dto.ModelCode))
        return Results.BadRequest(new { error = "Cần tỉnh From/To + nhà VC + model." });
    string pf = dto.ProvinceCodeFrom.Trim().ToUpperInvariant(), pt = dto.ProvinceCodeTo.Trim().ToUpperInvariant(),
           tr = dto.TransporterCode.Trim().ToUpperInvariant(), md = dto.ModelCode.Trim().ToUpperInvariant(),
           df = (dto.DistrictCodeFrom ?? "").Trim().ToUpperInvariant(), dt2 = (dto.DistrictCodeTo ?? "").Trim().ToUpperInvariant();
    // upsert theo khoá tuyến đầy đủ (tỉnh+huyện From/To + NVC + model)
    var f = await db.TranspFees.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ProvinceCodeFrom == pf && x.ProvinceCodeTo == pt
        && (x.DistrictCodeFrom ?? "") == df && (x.DistrictCodeTo ?? "") == dt2 && x.TransporterCode == tr && x.ModelCode == md);
    if (f is null) { f = new TranspFee { OrgId = t.OrgId, ProvinceCodeFrom = pf, ProvinceCodeTo = pt, DistrictCodeFrom = df, DistrictCodeTo = dt2, TransporterCode = tr, ModelCode = md }; db.TranspFees.Add(f); }
    f.ValFee = dto.ValFee; f.ExpectedDays = dto.ExpectedDays; f.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { f.ProvinceCodeFrom, f.ProvinceCodeTo, f.TransporterCode, f.ModelCode, f.ValFee, f.ExpectedDays });
}).RequireAuthorization();

// ===== Biên bản vận chuyển / giao nhận (TransportMinutes — port 1:1 FrmNewTransportMinutes/FrmMngTransportMinutes) =====
app.MapGet("/api/transminutes", async (AppDbContext db, ITenantContext t, string? status, string? dealer) =>
{
    var q = db.TransportMinutes.Where(m => m.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(m => m.Status == status);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(m => m.DealerCode == dealer);
    var items = await q.OrderByDescending(m => m.Id).Take(500).Select(m => new
    {
        m.TransportMinutesNo, m.DealerCode, m.TransporterCode, m.Status, m.CreatedAt, m.DecidedAt,
        cars = db.TransportMinutesCars.Count(c => c.OrgId == t.OrgId && c.MinutesId == m.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/transminutes", async (TransMinDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.TransporterCode))
        return Results.BadRequest(new { error = "Cần DealerCode và TransporterCode." });
    var vins = (dto.Cars ?? new List<TransMinCarDto>()).Where(c => !string.IsNullOrWhiteSpace(c.Vin)).ToList();
    if (vins.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    var dupe = vins.GroupBy(c => c.Vin.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"VIN {dupe.Key} bị trùng!" });
    var no = "TM" + DateTime.Now.ToString("yyMMddHHmmss");
    var m = new TransportMinutes { OrgId = t.OrgId, TransportMinutesNo = no, DealerCode = dto.DealerCode.Trim().ToUpperInvariant(), TransporterCode = dto.TransporterCode.Trim().ToUpperInvariant(), Status = "Pending" };
    db.TransportMinutes.Add(m); await db.SaveChangesAsync();
    foreach (var c in vins)
        db.TransportMinutesCars.Add(new TransportMinutesCar { OrgId = t.OrgId, MinutesId = m.Id, Vin = c.Vin.Trim().ToUpperInvariant(), DoNo = c.DoNo, ColorCode = c.ColorCode, EngineNo = c.EngineNo, DtlStatus = "Pending" });
    await db.SaveChangesAsync();
    return Results.Ok(new { m.TransportMinutesNo, m.DealerCode, m.TransporterCode, cars = vins.Count, status = m.Status });
}).RequireAuthorization();

app.MapGet("/api/transminutes/{no}/cars", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var m = await db.TransportMinutes.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TransportMinutesNo == no);
    if (m is null) return Results.NotFound(new { no });
    var cars = await db.TransportMinutesCars.Where(c => c.OrgId == t.OrgId && c.MinutesId == m.Id)
        .Select(c => new { c.Vin, c.DoNo, c.ColorCode, c.EngineNo, c.DtlStatus }).ToListAsync();
    return Results.Ok(new { m.TransportMinutesNo, m.Status, count = cars.Count, cars });
}).RequireAuthorization();

app.MapPost("/api/transminutes/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject")) return Results.BadRequest(new { error = "action = approve|reject" });
    no = no.Trim().ToUpperInvariant();
    var m = await db.TransportMinutes.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TransportMinutesNo == no);
    if (m is null) return Results.NotFound(new { no });
    if (m.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt/từ chối biên bản Đang xử lý." });
    m.Status = action == "approve" ? "Approved" : "Rejected"; m.DecidedAt = DateTime.Now;
    var dtl = m.Status;
    foreach (var c in await db.TransportMinutesCars.Where(c => c.OrgId == t.OrgId && c.MinutesId == m.Id).ToListAsync())
        c.DtlStatus = dtl;
    await db.SaveChangesAsync();
    return Results.Ok(new { m.TransportMinutesNo, status = m.Status });
}).RequireAuthorization();

// ===== Lịch ngày làm việc/nghỉ (Holiday — port 1:1 FrmCreateHoliday/FrmMngHoliday, Phase2) =====
app.MapGet("/api/holidays", async (AppDbContext db, ITenantContext t, int? year) =>
{
    var q = db.Holidays.Where(h => h.OrgId == t.OrgId);
    if (year is int y) q = q.Where(h => h.HolidayDate.Year == y);
    var items = await q.OrderBy(h => h.HolidayDate).Take(500).Select(h => new { date = h.HolidayDate, h.IsHoliday, h.Description }).ToListAsync();
    return Results.Ok(new { count = items.Count, holidays = items.Count(x => x.IsHoliday), items });
}).RequireAuthorization();

// Toggle 1 ngày (FrmMngHoliday.toggleDay → UpdateHoliday)
app.MapPost("/api/holidays/toggle", async (HolidayDto dto, AppDbContext db, ITenantContext t) =>
{
    if (dto.Date is null) return Results.BadRequest(new { error = "Cần Date." });
    var d = dto.Date.Value.Date;
    var h = await db.Holidays.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.HolidayDate == d);
    if (h is null) { h = new Holiday { OrgId = t.OrgId, HolidayDate = d }; db.Holidays.Add(h); }
    h.IsHoliday = dto.IsHoliday; h.Description = dto.Description;
    await db.SaveChangesAsync();
    return Results.Ok(new { date = h.HolidayDate, h.IsHoliday });
}).RequireAuthorization();

// Reset năm: sinh cuối tuần = nghỉ (FrmCreateHoliday.ResetHolidayForYear, arrDayOfWeek)
app.MapPost("/api/holidays/reset", async (HolidayResetDto dto, AppDbContext db, ITenantContext t) =>
{
    if (dto.Year is not int y || y < 2000 || y > 2100) return Results.BadRequest(new { error = "Year không hợp lệ." });
    var weekend = (dto.WeekendDays is { Count: > 0 } wd ? wd : new List<int> { 0, 6 }).ToHashSet(); // 0=CN,6=T7
    db.Holidays.RemoveRange(db.Holidays.Where(h => h.OrgId == t.OrgId && h.HolidayDate.Year == y));
    await db.SaveChangesAsync();
    int holidayCount = 0;
    for (var d = new DateTime(y, 1, 1); d.Year == y; d = d.AddDays(1))
    {
        bool isH = weekend.Contains((int)d.DayOfWeek);
        db.Holidays.Add(new Holiday { OrgId = t.OrgId, HolidayDate = d, IsHoliday = isH, Description = isH ? "Cuối tuần" : null });
        if (isH) holidayCount++;
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { year = y, days = DateTime.IsLeapYear(y) ? 366 : 365, holidays = holidayCount });
}).RequireAuthorization();

// ===== Kế hoạch vận chuyển xe từ kho (Sto_TranspPlan — port 1:1 FrmMngPlanTransport/FrmListPlanTransport) =====
app.MapGet("/api/transplans", async (AppDbContext db, ITenantContext t, string? status, string? dealer) =>
{
    var q = db.TransportPlans.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.Status == status);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(p => p.DealerCode == dealer);
    var items = await q.OrderByDescending(p => p.Id).Take(500).Select(p => new
    { p.VINPlan, p.Vin, p.ModelCode, p.DealerCode, p.StorageCode, p.FProvinceCode, p.TProvinceCode, p.TransporterCode, p.ExpectedDate, p.Status, p.ApprovedDate }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/transplans", async (TransPlanDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.VINPlan) || string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.ModelCode))
        return Results.BadRequest(new { error = "Cần VINPlan, DealerCode và ModelCode." });
    var vp = dto.VINPlan.Trim().ToUpperInvariant();
    var p = await db.TransportPlans.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.VINPlan == vp);
    if (p is null) { p = new TransportPlan { OrgId = t.OrgId, VINPlan = vp, Status = "Pending" }; db.TransportPlans.Add(p); }
    else if (p.Status == "Finished") return Results.BadRequest(new { error = "KH đã duyệt, không sửa được." });
    p.Vin = dto.Vin; p.ModelCode = dto.ModelCode.Trim().ToUpperInvariant(); p.DealerCode = dto.DealerCode.Trim().ToUpperInvariant();
    p.StorageCode = dto.StorageCode; p.FProvinceCode = dto.FProvinceCode; p.TProvinceCode = dto.TProvinceCode;
    p.TransporterCode = dto.TransporterCode; p.ExpectedDate = dto.ExpectedDate;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.VINPlan, p.DealerCode, p.ModelCode, status = p.Status });
}).RequireAuthorization();

// Duyệt KH (StoTranspPlanApproved → Finished)
app.MapPost("/api/transplans/{vinPlan}/approve", async (string vinPlan, AppDbContext db, ITenantContext t) =>
{
    vinPlan = vinPlan.Trim().ToUpperInvariant();
    var p = await db.TransportPlans.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.VINPlan == vinPlan);
    if (p is null) return Results.NotFound(new { vinPlan });
    if (p.Status == "Finished") return Results.BadRequest(new { error = "KH đã duyệt." });
    p.Status = "Finished"; p.ApprovedDate = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.VINPlan, status = p.Status });
}).RequireAuthorization();

// ===== Lệnh đặt xe từ nhà máy (POCommand — port 1:1 FrmNewHMCOrder/FrmMngHMCOrder, DMSales.Foton) =====
app.MapGet("/api/pocommands", async (AppDbContext db, ITenantContext t, string? status, string? month) =>
{
    var q = db.POCommands.Where(o => o.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(o => o.Status == status);
    if (!string.IsNullOrWhiteSpace(month)) q = q.Where(o => o.OrderMonth == month);
    var items = await q.OrderByDescending(o => o.Id).Take(500).Select(o => new
    {
        o.PoCmdCode, o.OrderMonth, o.Status, o.CreatedAt, o.SentAt,
        lines = db.POCommandLines.Count(l => l.OrgId == t.OrgId && l.PoCmdId == o.Id),
        totalQty = db.POCommandLines.Where(l => l.OrgId == t.OrgId && l.PoCmdId == o.Id).Sum(l => (int?)l.Quantity) ?? 0
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/pocommands", async (POCommandDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.OrderMonth)) return Results.BadRequest(new { error = "Cần OrderMonth (YYYYMM)." });
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.SpecCode) && l.Quantity > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng (SpecCode + Quantity > 0)." });
    var no = "POC" + DateTime.Now.ToString("yyMMddHHmmss");
    var o = new POCommand { OrgId = t.OrgId, PoCmdCode = no, OrderMonth = dto.OrderMonth.Trim(), Status = "Draft" };
    db.POCommands.Add(o); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.POCommandLines.Add(new POCommandLine { OrgId = t.OrgId, PoCmdId = o.Id, SpecCode = l.SpecCode.Trim().ToUpperInvariant(), SpecDesc = l.SpecDesc, ColorCode = l.ColorCode, PortCode = l.PortCode, PlantCode = l.PlantCode, Quantity = l.Quantity });
    await db.SaveChangesAsync();
    return Results.Ok(new { o.PoCmdCode, o.OrderMonth, lines = lines.Count, totalQty = lines.Sum(l => l.Quantity), status = o.Status });
}).RequireAuthorization();

app.MapGet("/api/pocommands/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var o = await db.POCommands.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PoCmdCode == no);
    if (o is null) return Results.NotFound(new { no });
    var lines = await db.POCommandLines.Where(l => l.OrgId == t.OrgId && l.PoCmdId == o.Id)
        .Select(l => new { l.SpecCode, l.SpecDesc, l.ColorCode, l.PortCode, l.PlantCode, l.Quantity }).ToListAsync();
    return Results.Ok(new { o.PoCmdCode, o.OrderMonth, o.Status, count = lines.Count, lines, totalQty = lines.Sum(x => x.Quantity) });
}).RequireAuthorization();

app.MapPost("/api/pocommands/{no}/send", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var o = await db.POCommands.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PoCmdCode == no);
    if (o is null) return Results.NotFound(new { no });
    if (o.Status != "Draft") return Results.BadRequest(new { error = "Chỉ gửi hãng lệnh Nháp." });
    o.Status = "Sent"; o.SentAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { o.PoCmdCode, status = o.Status });
}).RequireAuthorization();

// ===== Hóa đơn dịch vụ (Ser_Invoice — port 1:1 FrmInvoice) =====
app.MapGet("/api/serviceinvoices", async (AppDbContext db, ITenantContext t, string? status, string? ro) =>
{
    var q = db.ServiceInvoices.Where(i => i.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(i => i.Status == status);
    if (!string.IsNullOrWhiteSpace(ro)) q = q.Where(i => i.RONo.Contains(ro.ToUpper()));
    var items = await q.OrderByDescending(i => i.Id).Take(500).Select(i => new
    { i.InvoiceNo, i.RONo, i.SubTotal, i.VatPercent, i.VatAmount, i.DiscountAmount, i.TotalAmount, i.PaymentType, i.Status, i.PaidAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, totalRevenue = items.Where(x => x.Status == "Paid").Sum(x => x.TotalAmount), items });
}).RequireAuthorization();

// Lập hóa đơn từ RO: kéo Σ tiền công + Σ (SL×đơn giá PT), áp VAT + chiết khấu
app.MapPost("/api/serviceinvoices", async (ServiceInvoiceDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.RONo)) return Results.BadRequest(new { error = "Cần RONo." });
    var roNo = dto.RONo.Trim().ToUpperInvariant();
    var ro = await db.RepairOrders.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.RONo == roNo);
    if (ro is null) return Results.NotFound(new { error = $"Không tìm thấy RO {roNo}." });
    if (ro.Status is "HasRO" or "InGarage") return Results.BadRequest(new { error = "RO chưa sửa xong, chưa lập hóa đơn được." });
    if (await db.ServiceInvoices.AnyAsync(x => x.OrgId == t.OrgId && x.RONo == roNo && x.Status == "Paid"))
        return Results.BadRequest(new { error = "RO đã có hóa đơn thanh toán." });
    var svcTotal = await db.RoServiceItems.Where(s => s.OrgId == t.OrgId && s.RoId == ro.Id).SumAsync(s => (decimal?)s.Amount) ?? 0;
    var partTotal = await db.RoPartItems.Where(p => p.OrgId == t.OrgId && p.RoId == ro.Id).SumAsync(p => (decimal?)(p.NeedQty * p.UnitPrice)) ?? 0;
    var subTotal = svcTotal + partTotal;
    var vatPercent = dto.VatPercent < 0 ? 0 : dto.VatPercent;
    var discount = dto.DiscountAmount < 0 ? 0 : dto.DiscountAmount;
    var vatAmount = Math.Round(subTotal * vatPercent / 100m, 0);
    var total = subTotal + vatAmount - discount;
    if (total < 0) total = 0;
    var no = "INV" + DateTime.Now.ToString("yyMMddHHmmss");
    var inv = new ServiceInvoice
    {
        OrgId = t.OrgId, InvoiceNo = no, RONo = roNo, SubTotal = subTotal, VatPercent = vatPercent, VatAmount = vatAmount,
        DiscountAmount = discount, TotalAmount = total, PaymentType = dto.PaymentType, Status = "Draft"
    };
    db.ServiceInvoices.Add(inv); await db.SaveChangesAsync();
    return Results.Ok(new { inv.InvoiceNo, inv.RONo, inv.SubTotal, inv.VatAmount, inv.DiscountAmount, inv.TotalAmount, status = inv.Status });
}).RequireAuthorization();

// Thanh toán → Paid; nếu RO đang CheckEnd thì đẩy sang Paid (tích hợp workflow RO)
app.MapPost("/api/serviceinvoices/{no}/pay", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var inv = await db.ServiceInvoices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.InvoiceNo == no);
    if (inv is null) return Results.NotFound(new { no });
    if (inv.Status != "Draft") return Results.BadRequest(new { error = "Hóa đơn đã thanh toán." });
    inv.Status = "Paid"; inv.PaidAt = DateTime.Now;
    var ro = await db.RepairOrders.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.RONo == inv.RONo);
    string? roAdvanced = null;
    if (ro is not null && ro.Status == "CheckEnd") { ro.Status = "Paid"; roAdvanced = "Paid"; }
    await db.SaveChangesAsync();
    return Results.Ok(new { inv.InvoiceNo, status = inv.Status, roAdvancedTo = roAdvanced });
}).RequireAuthorization();

// ===== Chiến dịch dịch vụ (Ser_Campaign — port 1:1 FrmCampaignCreate) =====
app.MapGet("/api/campaigns", async (AppDbContext db, ITenantContext t, string? active) =>
{
    var q = db.Campaigns.Where(c => c.OrgId == t.OrgId);
    var now = DateTime.Now;
    if (active == "1") q = q.Where(c => c.StartDate <= now && (c.FinishDate == null || c.FinishDate >= now));
    var items = await q.OrderByDescending(c => c.Id).Take(500).Select(c => new
    {
        c.CamNo, c.CamName, c.StartDate, c.FinishDate, c.Content, c.Status,
        contacts = db.CampaignContacts.Count(x => x.OrgId == t.OrgId && x.CampaignId == c.Id),
        contacted = db.CampaignContacts.Count(x => x.OrgId == t.OrgId && x.CampaignId == c.Id && x.ContactStatus == "Contacted"),
        running = c.StartDate <= now && (c.FinishDate == null || c.FinishDate >= now)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/campaigns", async (CampaignDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.CamNo) || string.IsNullOrWhiteSpace(dto.CamName))
        return Results.BadRequest(new { error = "Cần CamNo và CamName." });
    if (dto.StartDate is null) return Results.BadRequest(new { error = "Cần StartDate." });
    if (dto.FinishDate is DateTime fd && fd < dto.StartDate.Value)
        return Results.BadRequest(new { error = "Ngày kết thúc phải ≥ ngày bắt đầu." });   // guard gốc FrmCampaignCreate
    var no = dto.CamNo.Trim().ToUpperInvariant();
    if (await db.Campaigns.AnyAsync(x => x.OrgId == t.OrgId && x.CamNo == no))
        return Results.BadRequest(new { error = $"Mã chiến dịch {no} đã tồn tại." });
    var c = new Campaign { OrgId = t.OrgId, CamNo = no, CamName = dto.CamName, StartDate = dto.StartDate.Value, FinishDate = dto.FinishDate, Content = dto.Content, Status = "1" };
    db.Campaigns.Add(c); await db.SaveChangesAsync();
    foreach (var ct in dto.Contacts ?? new())
        if (!string.IsNullOrWhiteSpace(ct.PlateNo) || !string.IsNullOrWhiteSpace(ct.CusName))
            db.CampaignContacts.Add(new CampaignContact { OrgId = t.OrgId, CampaignId = c.Id, PlateNo = ct.PlateNo, CusName = ct.CusName, Address = ct.Address, ContactStatus = "Pending" });
    await db.SaveChangesAsync();
    return Results.Ok(new { c.CamNo, c.CamName, contacts = (dto.Contacts ?? new()).Count });
}).RequireAuthorization();

app.MapGet("/api/campaigns/{no}/contacts", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var c = await db.Campaigns.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CamNo == no);
    if (c is null) return Results.NotFound(new { no });
    var contacts = await db.CampaignContacts.Where(x => x.OrgId == t.OrgId && x.CampaignId == c.Id)
        .Select(x => new { x.Id, x.PlateNo, x.CusName, x.Address, x.ContactStatus }).ToListAsync();
    return Results.Ok(new { c.CamNo, count = contacts.Count, contacts });
}).RequireAuthorization();

app.MapPost("/api/campaigns/contacts/{id:long}/contacted", async (long id, AppDbContext db, ITenantContext t) =>
{
    var ct = await db.CampaignContacts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Id == id);
    if (ct is null) return Results.NotFound(new { id });
    ct.ContactStatus = "Contacted";
    await db.SaveChangesAsync();
    return Results.Ok(new { ct.Id, status = ct.ContactStatus });
}).RequireAuthorization();

// ===== Nhóm sửa chữa (Ser_GroupRepair — port 1:1 FrmGroupRepairCreate) =====
app.MapGet("/api/grouprepairs", async (AppDbContext db, ITenantContext t) =>
{
    var items = await db.GroupRepairs.Where(g => g.OrgId == t.OrgId).OrderBy(g => g.GroupRCode)
        .Select(g => new { g.GroupRCode, g.GroupRName, g.Note, g.Status, engineers = db.ServiceEngineers.Count(e => e.OrgId == t.OrgId && e.GroupRCode == g.GroupRCode) }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/grouprepairs", async (GroupRepairDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.GroupRCode) || string.IsNullOrWhiteSpace(dto.GroupRName))
        return Results.BadRequest(new { error = "Cần GroupRCode và GroupRName." });
    var code = dto.GroupRCode.Trim().ToUpperInvariant();
    var g = await db.GroupRepairs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GroupRCode == code);
    if (g is null) { g = new GroupRepair { OrgId = t.OrgId, GroupRCode = code }; db.GroupRepairs.Add(g); }
    g.GroupRName = dto.GroupRName; g.Note = dto.Note; g.Status = dto.Status ?? "1"; g.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GroupRCode, g.GroupRName });
}).RequireAuthorization();

// ===== Kỹ thuật viên (Ser_Engineer — port 1:1 FrmEngineerCreate) =====
app.MapGet("/api/engineers", async (AppDbContext db, ITenantContext t, string? group, string? q) =>
{
    var query = db.ServiceEngineers.Where(e => e.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(group)) query = query.Where(e => e.GroupRCode == group);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(e => e.EngineerName.Contains(q) || e.EngineerNo.Contains(q.ToUpper()));
    var items = await query.OrderBy(e => e.EngineerNo).Take(500).Select(e => new { e.EngineerNo, e.EngineerName, e.GroupRCode, e.Note, e.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/engineers", async (EngineerDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.EngineerNo) || string.IsNullOrWhiteSpace(dto.EngineerName))
        return Results.BadRequest(new { error = "Cần EngineerNo và EngineerName." });
    var no = dto.EngineerNo.Trim().ToUpperInvariant();
    var e = await db.ServiceEngineers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.EngineerNo == no);
    if (e is null) { e = new ServiceEngineer { OrgId = t.OrgId, EngineerNo = no }; db.ServiceEngineers.Add(e); }
    e.EngineerName = dto.EngineerName; e.GroupRCode = dto.GroupRCode?.Trim().ToUpperInvariant(); e.Note = dto.Note; e.Status = dto.Status ?? "1"; e.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { e.EngineerNo, e.EngineerName, e.GroupRCode });
}).RequireAuthorization();

// ===== Yêu cầu báo giá phụ tùng (Req_PartPrice — port 1:1 FrmReq_PartPrice/Mng) =====
app.MapGet("/api/reqpartprices", async (AppDbContext db, ITenantContext t, string? dms, string? tst) =>
{
    var q = db.ReqPartPrices.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(dms)) q = q.Where(r => r.DMSStatus == dms);
    if (!string.IsNullOrWhiteSpace(tst)) q = q.Where(r => r.TSTStatus == tst);
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    {
        r.ReqNo, r.DMSStatus, r.TSTStatus, r.CreatedAt, r.QuotedAt,
        lines = db.ReqPartPriceLines.Count(l => l.OrgId == t.OrgId && l.ReqId == r.Id),
        quotedTotal = db.ReqPartPriceLines.Where(l => l.OrgId == t.OrgId && l.ReqId == r.Id).Sum(l => (decimal?)(l.ReqQty * l.QuotedPrice)) ?? 0
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/reqpartprices", async (ReqPartPriceDto dto, AppDbContext db, ITenantContext t) =>
{
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.PartCode) && l.ReqQty > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng PT (PartCode + ReqQty > 0)." });
    var no = "RQ" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new ReqPartPrice { OrgId = t.OrgId, ReqNo = no, DMSStatus = "P", TSTStatus = "Pending" };
    db.ReqPartPrices.Add(r); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.ReqPartPriceLines.Add(new ReqPartPriceLine { OrgId = t.OrgId, ReqId = r.Id, PartCode = l.PartCode.Trim().ToUpperInvariant(), PartName = l.PartName, ReqQty = l.ReqQty, QuotedPrice = 0 });
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReqNo, lines = lines.Count, dmsStatus = r.DMSStatus });
}).RequireAuthorization();

app.MapGet("/api/reqpartprices/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.ReqPartPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    var lines = await db.ReqPartPriceLines.Where(l => l.OrgId == t.OrgId && l.ReqId == r.Id)
        .Select(l => new { l.PartCode, l.PartName, l.ReqQty, l.QuotedPrice, lineTotal = l.ReqQty * l.QuotedPrice }).ToListAsync();
    return Results.Ok(new { r.ReqNo, r.DMSStatus, r.TSTStatus, count = lines.Count, lines, quotedTotal = lines.Sum(x => x.lineTotal) });
}).RequireAuthorization();

// DMS gửi (P→A)
app.MapPost("/api/reqpartprices/{no}/send", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.ReqPartPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.DMSStatus != "P") return Results.BadRequest(new { error = "Chỉ gửi YC Mới tạo." });
    r.DMSStatus = "A";
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReqNo, dmsStatus = r.DMSStatus });
}).RequireAuthorization();

// TST báo giá (điền QuotedPrice từng dòng) → TSTStatus Quoted
app.MapPost("/api/reqpartprices/{no}/quote", async (string no, ReqQuoteDto dto, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.ReqPartPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.DMSStatus != "A") return Results.BadRequest(new { error = "DMS chưa gửi YC." });
    if (r.TSTStatus == "Finished") return Results.BadRequest(new { error = "Đã hoàn tất." });
    var quotes = (dto.Quotes ?? new()).ToDictionary(x => (x.PartCode ?? "").Trim().ToUpperInvariant(), x => x.QuotedPrice);
    var lines = await db.ReqPartPriceLines.Where(l => l.OrgId == t.OrgId && l.ReqId == r.Id).ToListAsync();
    int filled = 0;
    foreach (var l in lines)
        if (quotes.TryGetValue(l.PartCode, out var price)) { l.QuotedPrice = price; filled++; }
    r.TSTStatus = "Quoted"; r.QuotedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReqNo, tstStatus = r.TSTStatus, filled });
}).RequireAuthorization();

// DMS chấp nhận → Finished
app.MapPost("/api/reqpartprices/{no}/finish", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.ReqPartPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.TSTStatus != "Quoted") return Results.BadRequest(new { error = "Chưa được báo giá (Quoted)." });
    r.TSTStatus = "Finished"; r.DMSStatus = "F";
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReqNo, dmsStatus = r.DMSStatus, tstStatus = r.TSTStatus });
}).RequireAuthorization();

// ===== Thanh toán nhà cung cấp (Ser_SupplierPayment — port 1:1 FrmSer_SupplierPayment) =====
app.MapGet("/api/supplierpayments", async (AppDbContext db, ITenantContext t, string? status, string? supplier) =>
{
    var q = db.SupplierPayments.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.Status == status);
    if (!string.IsNullOrWhiteSpace(supplier)) q = q.Where(p => p.SupplierCode == supplier);
    var items = await q.OrderByDescending(p => p.Id).Take(500).Select(p => new
    { p.PaymentNo, p.SupplierCode, p.OrderPartNo, p.Amount, p.PaymentDate, p.Status, p.ApprovedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, total = items.Sum(x => x.Amount), approved = items.Where(x => x.Status == "A").Sum(x => x.Amount), items });
}).RequireAuthorization();

app.MapPost("/api/supplierpayments", async (SupplierPaymentDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SupplierCode)) return Results.BadRequest(new { error = "Cần SupplierCode." });
    decimal amount = dto.Amount;
    string? orderNo = null;
    if (!string.IsNullOrWhiteSpace(dto.OrderPartNo))
    {
        orderNo = dto.OrderPartNo.Trim().ToUpperInvariant();
        var order = await db.OrderParts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.OrderPartNo == orderNo);
        if (order is null) return Results.BadRequest(new { error = $"Không tìm thấy đơn đặt {orderNo}." });
        if (order.OrderPartStatus != "Finished") return Results.BadRequest(new { error = "Chỉ thanh toán đơn đã Hoàn thành." });
        // amount mặc định = tổng đơn nếu không nhập
        if (amount <= 0)
            amount = await db.OrderPartLines.Where(l => l.OrgId == t.OrgId && l.OrderPartId == order.Id).SumAsync(l => (decimal?)(l.OrderQty * l.Price)) ?? 0;
    }
    if (amount <= 0) return Results.BadRequest(new { error = "Cần số tiền > 0." });
    var no = "SP" + DateTime.Now.ToString("yyMMddHHmmss");
    var p = new SupplierPayment { OrgId = t.OrgId, PaymentNo = no, SupplierCode = dto.SupplierCode.Trim().ToUpperInvariant(), OrderPartNo = orderNo, Amount = amount, PaymentDate = dto.PaymentDate ?? DateTime.Now, Status = "P" };
    db.SupplierPayments.Add(p); await db.SaveChangesAsync();
    return Results.Ok(new { p.PaymentNo, p.SupplierCode, p.OrderPartNo, p.Amount, status = p.Status });
}).RequireAuthorization();

app.MapPost("/api/supplierpayments/{no}/approve", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var p = await db.SupplierPayments.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PaymentNo == no);
    if (p is null) return Results.NotFound(new { no });
    if (p.Status != "P") return Results.BadRequest(new { error = "Chỉ duyệt phiếu Mới tạo." });
    p.Status = "A"; p.ApprovedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PaymentNo, status = p.Status });
}).RequireAuthorization();

// ===== Khiếu nại đơn đặt phụ tùng (Ser_OrderComplain — port 1:1 FrmSer_OrderComplain/Mng) =====
app.MapGet("/api/ordercomplains", async (AppDbContext db, ITenantContext t, string? dms, string? tst, string? order) =>
{
    var q = db.OrderComplains.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(dms)) q = q.Where(c => c.DMSStatus == dms);
    if (!string.IsNullOrWhiteSpace(tst)) q = q.Where(c => c.TSTStatus == tst);
    if (!string.IsNullOrWhiteSpace(order)) q = q.Where(c => c.OrderPartNo.Contains(order.ToUpper()));
    var items = await q.OrderByDescending(c => c.Id).Take(500).Select(c => new
    { c.ComplainNo, c.OrderPartNo, c.ComplainType, c.Content, c.DMSStatus, c.TSTStatus, c.Resolution, c.CreatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/ordercomplains", async (OrderComplainDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.OrderPartNo)) return Results.BadRequest(new { error = "Cần OrderPartNo." });
    var orderNo = dto.OrderPartNo.Trim().ToUpperInvariant();
    var exists = await db.OrderParts.AnyAsync(x => x.OrgId == t.OrgId && x.OrderPartNo == orderNo);
    if (!exists) return Results.BadRequest(new { error = $"Không tìm thấy đơn đặt {orderNo}." });
    var no = "CMP" + DateTime.Now.ToString("yyMMddHHmmss");
    var c = new OrderComplain { OrgId = t.OrgId, ComplainNo = no, OrderPartNo = orderNo, ComplainType = dto.ComplainType, Content = dto.Content, DMSStatus = "P", TSTStatus = "" };
    db.OrderComplains.Add(c); await db.SaveChangesAsync();
    return Results.Ok(new { c.ComplainNo, c.OrderPartNo, dmsStatus = c.DMSStatus });
}).RequireAuthorization();

// DMS gửi (P→A); TST tiếp nhận/xử lý/giải quyết
app.MapPost("/api/ordercomplains/{no}/{action}", async (string no, string action, OrderComplainActDto dto, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("send" or "receive" or "process" or "resolve")) return Results.BadRequest(new { error = "action = send|receive|process|resolve" });
    no = no.Trim().ToUpperInvariant();
    var c = await db.OrderComplains.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ComplainNo == no);
    if (c is null) return Results.NotFound(new { no });
    if (action == "send")
    {
        if (c.DMSStatus != "P") return Results.BadRequest(new { error = "Chỉ gửi khiếu nại Mới tạo." });
        c.DMSStatus = "A";
    }
    else if (action == "receive")
    {
        if (c.DMSStatus != "A") return Results.BadRequest(new { error = "DMS chưa gửi khiếu nại." });
        if (c.TSTStatus != "") return Results.BadRequest(new { error = "Đã tiếp nhận." });
        c.TSTStatus = "Processing";
    }
    else if (action == "process")
    {
        if (c.TSTStatus != "Processing") return Results.BadRequest(new { error = "Chưa tiếp nhận (Processing)." });
        c.TSTStatus = "Pending";
    }
    else // resolve
    {
        if (c.TSTStatus != "Pending") return Results.BadRequest(new { error = "Chưa đang xử lý (Pending)." });
        c.TSTStatus = "Resolved"; c.Resolution = dto.Resolution;
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { c.ComplainNo, dmsStatus = c.DMSStatus, tstStatus = c.TSTStatus });
}).RequireAuthorization();

// ===== Đơn đặt phụ tùng từ NCC (Ser_Order_Part — port 1:1 FrmSer_Order_Part) =====
app.MapGet("/api/orderparts", async (AppDbContext db, ITenantContext t, string? status, string? supplier) =>
{
    var q = db.OrderParts.Where(o => o.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(o => o.OrderPartStatus == status);
    if (!string.IsNullOrWhiteSpace(supplier)) q = q.Where(o => o.SupplierCode == supplier);
    var items = await q.OrderByDescending(o => o.Id).Take(500).Select(o => new
    {
        o.OrderPartNo, o.SupplierCode, o.WarehouseCode, o.OrderPartStatus, o.CreatedAt, o.SentAt, o.FinishedAt,
        lines = db.OrderPartLines.Count(l => l.OrgId == t.OrgId && l.OrderPartId == o.Id),
        total = db.OrderPartLines.Where(l => l.OrgId == t.OrgId && l.OrderPartId == o.Id).Sum(l => (decimal?)(l.OrderQty * l.Price)) ?? 0
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/orderparts", async (OrderPartDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SupplierCode)) return Results.BadRequest(new { error = "Cần SupplierCode (nhà cung cấp)." });
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.PartCode) && l.OrderQty > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng phụ tùng (PartCode + OrderQty > 0)." });
    var no = "OP" + DateTime.Now.ToString("yyMMddHHmmss");
    var o = new OrderPart { OrgId = t.OrgId, OrderPartNo = no, SupplierCode = dto.SupplierCode.Trim().ToUpperInvariant(), WarehouseCode = dto.WarehouseCode, OrderPartStatus = "Pending" };
    db.OrderParts.Add(o); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.OrderPartLines.Add(new OrderPartLine { OrgId = t.OrgId, OrderPartId = o.Id, PartCode = l.PartCode.Trim().ToUpperInvariant(), PartName = l.PartName, OrderQty = l.OrderQty, Price = l.Price });
    await db.SaveChangesAsync();
    return Results.Ok(new { o.OrderPartNo, o.SupplierCode, lines = lines.Count, status = o.OrderPartStatus });
}).RequireAuthorization();

app.MapGet("/api/orderparts/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var o = await db.OrderParts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.OrderPartNo == no);
    if (o is null) return Results.NotFound(new { no });
    var lines = await db.OrderPartLines.Where(l => l.OrgId == t.OrgId && l.OrderPartId == o.Id)
        .Select(l => new { l.PartCode, l.PartName, l.OrderQty, l.Price, lineTotal = l.OrderQty * l.Price }).ToListAsync();
    return Results.Ok(new { o.OrderPartNo, o.SupplierCode, o.OrderPartStatus, count = lines.Count, lines, total = lines.Sum(x => x.lineTotal) });
}).RequireAuthorization();

// Gửi NCC (approve) / Hoàn thành (finish) — tiến đúng chuỗi Pending→Approved→Finished
app.MapPost("/api/orderparts/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "finish")) return Results.BadRequest(new { error = "action = approve|finish" });
    no = no.Trim().ToUpperInvariant();
    var o = await db.OrderParts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.OrderPartNo == no);
    if (o is null) return Results.NotFound(new { no });
    if (action == "approve")
    {
        if (o.OrderPartStatus != "Pending") return Results.BadRequest(new { error = "Chỉ gửi NCC đơn Mới tạo." });
        o.OrderPartStatus = "Approved"; o.SentAt = DateTime.Now;
    }
    else
    {
        if (o.OrderPartStatus != "Approved") return Results.BadRequest(new { error = "Chỉ hoàn thành đơn Đã gửi NCC." });
        o.OrderPartStatus = "Finished"; o.FinishedAt = DateTime.Now;
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { o.OrderPartNo, status = o.OrderPartStatus });
}).RequireAuthorization();

// ===== Khách hàng dịch vụ (Ser_Customer — port 1:1 FrmCustomerInfo) =====
app.MapGet("/api/servicecustomers", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.ServiceCustomers.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q))
        query = query.Where(c => c.CusName.Contains(q) || c.CusCode.Contains(q.ToUpper())
            || (c.Mobile != null && c.Mobile.Contains(q)) || (c.Tel != null && c.Tel.Contains(q)) || (c.TaxCode != null && c.TaxCode.Contains(q)));
    var items = await query.OrderBy(c => c.CusName).Take(500).Select(c => new
    { c.CusCode, c.CusName, c.CusTypeID, c.Address, c.Mobile, c.Tel, c.Email, c.TaxCode, c.Sex, c.DOB, c.ContName, c.ContMobile }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/servicecustomers", async (ServiceCustomerDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.CusName)) return Results.BadRequest(new { error = "Cần CusName." });
    if (string.IsNullOrWhiteSpace(dto.Mobile) && string.IsNullOrWhiteSpace(dto.Tel)) return Results.BadRequest(new { error = "Cần SĐT di động hoặc cố định." });
    var code = string.IsNullOrWhiteSpace(dto.CusCode) ? "CUS" + DateTime.Now.ToString("yyMMddHHmmss") : dto.CusCode.Trim().ToUpperInvariant();
    var c = await db.ServiceCustomers.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CusCode == code);
    if (c is null) { c = new ServiceCustomer { OrgId = t.OrgId, CusCode = code }; db.ServiceCustomers.Add(c); }
    c.CusName = dto.CusName; c.CusTypeID = dto.CusTypeID; c.Address = dto.Address; c.Mobile = dto.Mobile; c.Tel = dto.Tel;
    c.Email = dto.Email; c.TaxCode = dto.TaxCode; c.Sex = dto.Sex; c.DOB = dto.DOB;
    c.ContName = dto.ContName; c.ContMobile = dto.ContMobile; c.ContTel = dto.ContTel; c.ContEmail = dto.ContEmail; c.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { c.CusCode, c.CusName });
}).RequireAuthorization();

// ===== Chăm sóc khách hàng (Ser_CustomerCare — port 1:1 FrmCustomerCare) =====
string[] _careTypes = { "CARE24H", "CARE72H", "DOB", "MAINT" };
app.MapGet("/api/customercares", async (AppDbContext db, ITenantContext t, string? type, string? status, string? plate) =>
{
    var q = db.CustomerCares.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(type)) q = q.Where(c => c.CareType == type);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(c => c.Status == status);
    if (!string.IsNullOrWhiteSpace(plate)) q = q.Where(c => c.PlateNo != null && c.PlateNo.Contains(plate.ToUpper()));
    var items = await q.OrderByDescending(c => c.Id).Take(500).Select(c => new
    { c.CareNo, c.CareType, c.RONo, c.PlateNo, c.CusName, c.CusPhone, c.ContactDate, c.Status, c.Result, c.ContactedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, pending = items.Count(x => x.Status == "Pending"), items });
}).RequireAuthorization();

app.MapPost("/api/customercares", async (CustomerCareDto dto, AppDbContext db, ITenantContext t) =>
{
    var type = string.IsNullOrWhiteSpace(dto.CareType) ? "CARE24H" : dto.CareType.Trim().ToUpperInvariant();
    if (!_careTypes.Contains(type)) return Results.BadRequest(new { error = "CareType = CARE24H|CARE72H|DOB|MAINT" });
    if (string.IsNullOrWhiteSpace(dto.PlateNo) && string.IsNullOrWhiteSpace(dto.CusPhone))
        return Results.BadRequest(new { error = "Cần biển số hoặc SĐT khách." });
    var no = "CC" + DateTime.Now.ToString("yyMMddHHmmss");
    var c = new CustomerCare
    {
        OrgId = t.OrgId, CareNo = no, CareType = type, RONo = dto.RONo, PlateNo = dto.PlateNo?.Trim().ToUpperInvariant(),
        CusName = dto.CusName, CusPhone = dto.CusPhone, ContactDate = dto.ContactDate, Status = "Pending"
    };
    db.CustomerCares.Add(c); await db.SaveChangesAsync();
    return Results.Ok(new { c.CareNo, c.CareType, status = c.Status });
}).RequireAuthorization();

// Ghi nhận đã liên hệ (kết quả) → Contacted
app.MapPost("/api/customercares/{no}/contact", async (string no, CareContactDto dto, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var c = await db.CustomerCares.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CareNo == no);
    if (c is null) return Results.NotFound(new { no });
    if (c.Status == "Closed") return Results.BadRequest(new { error = "Phiếu đã đóng." });
    c.Status = "Contacted"; c.Result = dto.Result; c.ContactedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { c.CareNo, status = c.Status });
}).RequireAuthorization();

// Đóng phiếu → Closed
app.MapPost("/api/customercares/{no}/close", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var c = await db.CustomerCares.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.CareNo == no);
    if (c is null) return Results.NotFound(new { no });
    if (c.Status != "Contacted") return Results.BadRequest(new { error = "Chỉ đóng phiếu Đã liên hệ." });
    c.Status = "Closed";
    await db.SaveChangesAsync();
    return Results.Ok(new { c.CareNo, status = c.Status });
}).RequireAuthorization();

// ===== Xe của khách hàng (Ser_Car — port 1:1 FrmCustomerCar) =====
app.MapGet("/api/customercars", async (AppDbContext db, ITenantContext t, string? plate, string? vin, string? cus) =>
{
    var q = db.CustomerCars.Where(c => c.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(plate)) q = q.Where(c => c.PlateNo.Contains(plate.ToUpper()));
    if (!string.IsNullOrWhiteSpace(vin)) q = q.Where(c => c.Vin.Contains(vin.ToUpper()));
    if (!string.IsNullOrWhiteSpace(cus)) q = q.Where(c => (c.CusName != null && c.CusName.Contains(cus)) || (c.CusPhone != null && c.CusPhone.Contains(cus)));
    var items = await q.OrderBy(c => c.PlateNo).Take(500).Select(c => new
    { c.Vin, c.PlateNo, c.FrameNo, c.EngineNo, c.ModelCode, c.ColorCode, c.PlateColorCode, c.CusCode, c.CusName, c.CusPhone, c.SaleDate }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/customercars", async (CustomerCarDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin) && string.IsNullOrWhiteSpace(dto.PlateNo))
        return Results.BadRequest(new { error = "Cần VIN hoặc biển số." });
    var vin = (dto.Vin ?? "").Trim().ToUpperInvariant();
    var plate = (dto.PlateNo ?? "").Trim().ToUpperInvariant();
    // upsert theo VIN nếu có, ngược lại theo biển số
    CustomerCar? c = null;
    if (vin.Length > 0) c = await db.CustomerCars.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Vin == vin);
    if (c is null && plate.Length > 0) c = await db.CustomerCars.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Vin == "" && x.PlateNo == plate);
    if (c is null) { c = new CustomerCar { OrgId = t.OrgId, Vin = vin, PlateNo = plate }; db.CustomerCars.Add(c); }
    else { if (vin.Length > 0) c.Vin = vin; if (plate.Length > 0) c.PlateNo = plate; }
    c.FrameNo = dto.FrameNo; c.EngineNo = dto.EngineNo; c.ModelCode = dto.ModelCode; c.ColorCode = dto.ColorCode; c.PlateColorCode = dto.PlateColorCode;
    c.CusCode = dto.CusCode; c.CusName = dto.CusName; c.CusPhone = dto.CusPhone; c.SaleDate = dto.SaleDate; c.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { c.Vin, c.PlateNo, c.CusName });
}).RequireAuthorization();

// ===== Giá bán phụ tùng theo ngày hiệu lực (Ser_Inv_PartPrice — port 1:1 FrmPartPriceCreate) =====
app.MapGet("/api/partprices", async (AppDbContext db, ITenantContext t, string? part, string? onDate) =>
{
    var q = db.PartPrices.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(part)) q = q.Where(p => p.PartCode.Contains(part.ToUpper()));
    var items = await q.OrderBy(p => p.PartCode).ThenByDescending(p => p.EffectiveDate).Take(1000).Select(p => new
    { p.PartCode, p.PartName, p.Price, p.VAT, p.PriceVAT, p.EffectiveDate, p.Status }).ToListAsync();
    object? applicable = null;
    if (!string.IsNullOrWhiteSpace(part) && DateTime.TryParse(onDate, out var od))
        applicable = items.Where(x => x.PartCode == part.Trim().ToUpperInvariant() && x.EffectiveDate <= od)
            .OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
    return Results.Ok(new { count = items.Count, applicable, items });
}).RequireAuthorization();

app.MapPost("/api/partprices", async (PartPriceDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.PartCode)) return Results.BadRequest(new { error = "Cần PartCode." });
    if (dto.EffectiveDate is null) return Results.BadRequest(new { error = "Cần EffectiveDate." });
    var code = dto.PartCode.Trim().ToUpperInvariant();
    var ed = dto.EffectiveDate.Value.Date;
    var p = await db.PartPrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PartCode == code && x.EffectiveDate == ed);
    if (p is null) { p = new PartPrice { OrgId = t.OrgId, PartCode = code, EffectiveDate = ed }; db.PartPrices.Add(p); }
    p.PartName = dto.PartName; p.Price = dto.Price; p.VAT = dto.VAT; p.PriceVAT = Math.Round(dto.Price * (1 + dto.VAT / 100m), 2);
    p.Status = dto.Status ?? "1"; p.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PartCode, p.Price, p.VAT, p.PriceVAT, p.EffectiveDate });
}).RequireAuthorization();

// ===== Phiếu xuất kho phụ tùng (Ser_Inv_StockOut — port 1:1 FrmStockOutCreate), TRỪ tồn PartStock =====
app.MapGet("/api/stockouts", async (AppDbContext db, ITenantContext t, string? status, string? warehouse) =>
{
    var q = db.PartStockOuts.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(s => s.Status == status);
    if (!string.IsNullOrWhiteSpace(warehouse)) q = q.Where(s => s.WarehouseCode == warehouse);
    var items = await q.OrderByDescending(s => s.Id).Take(500).Select(s => new
    {
        s.StockOutNo, s.StockOutDate, s.StockOutType, s.WarehouseCode, s.Reason, s.Status, s.PostedAt,
        lines = db.PartStockOutLines.Count(l => l.OrgId == t.OrgId && l.StockOutId == s.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/stockouts", async (StockOutDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.WarehouseCode)) return Results.BadRequest(new { error = "Cần WarehouseCode." });
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.PartCode) && l.Quantity > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng phụ tùng (PartCode + Quantity > 0)." });
    var no = "SO" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new PartStockOut { OrgId = t.OrgId, StockOutNo = no, StockOutDate = dto.StockOutDate ?? DateTime.Now, StockOutType = dto.StockOutType, WarehouseCode = dto.WarehouseCode.Trim().ToUpperInvariant(), Reason = dto.Reason, Status = "Draft" };
    db.PartStockOuts.Add(h); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.PartStockOutLines.Add(new PartStockOutLine { OrgId = t.OrgId, StockOutId = h.Id, PartCode = l.PartCode.Trim().ToUpperInvariant(), PartName = l.PartName, Location = l.Location, Quantity = l.Quantity });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.StockOutNo, h.WarehouseCode, lines = lines.Count, status = h.Status });
}).RequireAuthorization();

app.MapGet("/api/stockouts/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.PartStockOuts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.StockOutNo == no);
    if (h is null) return Results.NotFound(new { no });
    var lines = await db.PartStockOutLines.Where(l => l.OrgId == t.OrgId && l.StockOutId == h.Id)
        .Select(l => new { l.PartCode, l.PartName, l.Location, l.Quantity }).ToListAsync();
    return Results.Ok(new { h.StockOutNo, h.WarehouseCode, h.Status, count = lines.Count, lines });
}).RequireAuthorization();

// Ghi sổ: TRỪ tồn PartStock; guard tồn không đủ (kiểm TẤT CẢ dòng trước khi trừ)
app.MapPost("/api/stockouts/{no}/post", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.PartStockOuts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.StockOutNo == no);
    if (h is null) return Results.NotFound(new { no });
    if (h.Status != "Draft") return Results.BadRequest(new { error = "Chỉ ghi sổ phiếu Draft." });
    var lines = await db.PartStockOutLines.Where(l => l.OrgId == t.OrgId && l.StockOutId == h.Id).ToListAsync();
    // kiểm tồn đủ trước
    foreach (var l in lines)
    {
        var loc = l.Location ?? "";
        var stock = await db.PartStocks.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.WarehouseCode == h.WarehouseCode && x.PartCode == l.PartCode && (x.Location ?? "") == loc);
        var onhand = stock?.OnHand ?? 0;
        if (onhand < l.Quantity)
            return Results.BadRequest(new { error = $"Tồn không đủ cho {l.PartCode}@{loc}: cần {l.Quantity}, còn {onhand}." });
    }
    // trừ tồn
    foreach (var l in lines)
    {
        var loc = l.Location ?? "";
        var stock = await db.PartStocks.FirstAsync(x => x.OrgId == t.OrgId && x.WarehouseCode == h.WarehouseCode && x.PartCode == l.PartCode && (x.Location ?? "") == loc);
        stock.OnHand -= l.Quantity; stock.UpdatedAt = DateTime.Now;
    }
    h.Status = "Posted"; h.PostedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { h.StockOutNo, status = h.Status, postedLines = lines.Count });
}).RequireAuthorization();

// ===== Phiếu nhập kho phụ tùng (Ser_Inv_StockIn — port 1:1 FrmStockInCreate) + tồn kho PartStock =====
app.MapGet("/api/stockins", async (AppDbContext db, ITenantContext t, string? status, string? warehouse) =>
{
    var q = db.PartStockIns.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(s => s.Status == status);
    if (!string.IsNullOrWhiteSpace(warehouse)) q = q.Where(s => s.WarehouseCode == warehouse);
    var items = await q.OrderByDescending(s => s.Id).Take(500).Select(s => new
    {
        s.StockInNo, s.StockInDate, s.StockInType, s.WarehouseCode, s.Staff, s.Status, s.PostedAt,
        lines = db.PartStockInLines.Count(l => l.OrgId == t.OrgId && l.StockInId == s.Id),
        total = db.PartStockInLines.Where(l => l.OrgId == t.OrgId && l.StockInId == s.Id).Sum(l => (decimal?)(l.Quantity * l.Price)) ?? 0
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/stockins", async (StockInDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.WarehouseCode)) return Results.BadRequest(new { error = "Cần WarehouseCode." });
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.PartCode) && l.Quantity > 0).ToList();
    if (lines.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 dòng phụ tùng (PartCode + Quantity > 0)." });
    var no = "SI" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new PartStockIn { OrgId = t.OrgId, StockInNo = no, StockInDate = dto.StockInDate ?? DateTime.Now, StockInType = dto.StockInType, WarehouseCode = dto.WarehouseCode.Trim().ToUpperInvariant(), Staff = dto.Staff, Status = "Draft" };
    db.PartStockIns.Add(h); await db.SaveChangesAsync();
    foreach (var l in lines)
        db.PartStockInLines.Add(new PartStockInLine { OrgId = t.OrgId, StockInId = h.Id, PartCode = l.PartCode.Trim().ToUpperInvariant(), PartName = l.PartName, Location = l.Location, Quantity = l.Quantity, Price = l.Price, VAT = l.VAT });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.StockInNo, h.WarehouseCode, lines = lines.Count, status = h.Status });
}).RequireAuthorization();

app.MapGet("/api/stockins/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.PartStockIns.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.StockInNo == no);
    if (h is null) return Results.NotFound(new { no });
    var lines = await db.PartStockInLines.Where(l => l.OrgId == t.OrgId && l.StockInId == h.Id)
        .Select(l => new { l.PartCode, l.PartName, l.Location, l.Quantity, l.Price, l.VAT, lineTotal = l.Quantity * l.Price }).ToListAsync();
    return Results.Ok(new { h.StockInNo, h.WarehouseCode, h.Status, count = lines.Count, lines });
}).RequireAuthorization();

// Ghi sổ: tăng tồn PartStock (integration thật)
app.MapPost("/api/stockins/{no}/post", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.PartStockIns.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.StockInNo == no);
    if (h is null) return Results.NotFound(new { no });
    if (h.Status != "Draft") return Results.BadRequest(new { error = "Chỉ ghi sổ phiếu Draft." });
    var lines = await db.PartStockInLines.Where(l => l.OrgId == t.OrgId && l.StockInId == h.Id).ToListAsync();
    foreach (var l in lines)
    {
        var loc = l.Location ?? "";
        var stock = await db.PartStocks.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.WarehouseCode == h.WarehouseCode && x.PartCode == l.PartCode && (x.Location ?? "") == loc);
        if (stock is null) { stock = new PartStock { OrgId = t.OrgId, WarehouseCode = h.WarehouseCode, PartCode = l.PartCode, PartName = l.PartName, Location = l.Location, OnHand = 0 }; db.PartStocks.Add(stock); }
        stock.OnHand += l.Quantity; stock.PartName = l.PartName ?? stock.PartName; stock.UpdatedAt = DateTime.Now;
    }
    h.Status = "Posted"; h.PostedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { h.StockInNo, status = h.Status, postedLines = lines.Count });
}).RequireAuthorization();

// Tồn kho phụ tùng (Ser_Inv_PartStock — báo cáo tồn, đọc bởi FrmPartStockSearch/ReportStockBalance)
app.MapGet("/api/partstock", async (AppDbContext db, ITenantContext t, string? warehouse, string? part) =>
{
    var q = db.PartStocks.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(warehouse)) q = q.Where(s => s.WarehouseCode == warehouse);
    if (!string.IsNullOrWhiteSpace(part)) q = q.Where(s => s.PartCode.Contains(part.ToUpper()));
    var items = await q.OrderBy(s => s.PartCode).Take(1000).Select(s => new { s.WarehouseCode, s.PartCode, s.PartName, s.Location, s.OnHand }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

// ===== Phiếu tiếp nhận xe dịch vụ (Ser_ReceptionF — port 1:1 FrmSerReceptionFMng) =====
app.MapGet("/api/receptions", async (AppDbContext db, ITenantContext t, string? status, string? plate) =>
{
    var q = db.Receptions.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(r => r.Status == status);
    if (!string.IsNullOrWhiteSpace(plate)) q = q.Where(r => r.PlateNo.Contains(plate.ToUpper()));
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    { r.ReceptionFNo, r.PlateNo, r.ModelName, r.CusName, r.CusPhoneNo, r.CusRequest, r.RONO, r.Status, r.CreatedAt, r.DeliveredAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, pending = items.Count(x => x.Status == "Pending"), items });
}).RequireAuthorization();

app.MapPost("/api/receptions", async (ReceptionDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.PlateNo)) return Results.BadRequest(new { error = "Cần biển số (PlateNo)." });
    var no = "RCP" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new Reception
    {
        OrgId = t.OrgId, ReceptionFNo = no, PlateNo = dto.PlateNo.Trim().ToUpperInvariant(), ModelName = dto.ModelName,
        CusName = dto.CusName, CusAddress = dto.CusAddress, CusPhoneNo = dto.CusPhoneNo, CusRequest = dto.CusRequest, Status = "Pending"
    };
    db.Receptions.Add(r); await db.SaveChangesAsync();
    return Results.Ok(new { r.ReceptionFNo, r.PlateNo, status = r.Status });
}).RequireAuthorization();

// Gắn RO (kiểm tra RO tồn tại — tích hợp RepairOrder)
app.MapPost("/api/receptions/{no}/linkro", async (string no, ReceptionLinkDto dto, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.Receptions.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReceptionFNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (string.IsNullOrWhiteSpace(dto.RONO)) return Results.BadRequest(new { error = "Cần RONO." });
    var roNo = dto.RONO.Trim().ToUpperInvariant();
    var roExists = await db.RepairOrders.AnyAsync(x => x.OrgId == t.OrgId && x.RONo == roNo);
    if (!roExists) return Results.BadRequest(new { error = $"Không tìm thấy RO {roNo}." });
    r.RONO = roNo;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReceptionFNo, r.RONO });
}).RequireAuthorization();

// Giao xe (Approved) — cần đã gắn RO
app.MapPost("/api/receptions/{no}/deliver", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.Receptions.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReceptionFNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.Status != "Pending") return Results.BadRequest(new { error = "Chỉ giao xe cho phiếu Tiếp nhận." });
    if (string.IsNullOrWhiteSpace(r.RONO)) return Results.BadRequest(new { error = "Chưa gắn RO, không thể giao xe." });
    r.Status = "Approved"; r.DeliveredAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.ReceptionFNo, status = r.Status });
}).RequireAuthorization();

// ===== Phiếu xuất kho phụ tùng cho RO (Ser_RO_StockRequisition — port 1:1 FrmROStockRequisition) =====
app.MapGet("/api/stockreqs", async (AppDbContext db, ITenantContext t, string? status, string? ro) =>
{
    var q = db.StockReqs.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(s => s.Status == status);
    if (!string.IsNullOrWhiteSpace(ro)) q = q.Where(s => s.RONo.Contains(ro.ToUpper()));
    var items = await q.OrderByDescending(s => s.Id).Take(500).Select(s => new
    {
        s.ReqNo, s.RONo, s.Status, s.CreatedAt, s.IssuedAt,
        lines = db.StockReqLines.Count(l => l.OrgId == t.OrgId && l.ReqId == s.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

// Tạo phiếu; nếu FromRO=true tự kéo phụ tùng của RO (tích hợp thật với RepairOrder)
app.MapPost("/api/stockreqs", async (StockReqDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.RONo)) return Results.BadRequest(new { error = "Cần RONo." });
    var roNo = dto.RONo.Trim().ToUpperInvariant();
    var ro = await db.RepairOrders.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.RONo == roNo);
    if (ro is null) return Results.NotFound(new { error = $"Không tìm thấy RO {roNo}." });
    var lines = (dto.Lines ?? new()).Where(l => !string.IsNullOrWhiteSpace(l.PartCode)).ToList();
    List<StockReqLine> pulled = new();
    if (dto.FromRO)
    {
        var roParts = await db.RoPartItems.Where(p => p.OrgId == t.OrgId && p.RoId == ro.Id).ToListAsync();
        pulled = roParts.Select(p => new StockReqLine { OrgId = t.OrgId, PartCode = p.PartCode, PartName = p.PartName, Quantity = p.NeedQty, Unit = p.Unit }).ToList();
    }
    if (lines.Count == 0 && pulled.Count == 0) return Results.BadRequest(new { error = "Không có dòng phụ tùng (đặt fromRO=true để kéo từ RO, hoặc gửi lines)." });
    var no = "PX-" + roNo;
    var h = new StockReq { OrgId = t.OrgId, ReqNo = no, RONo = roNo, Status = "Draft" };
    db.StockReqs.Add(h); await db.SaveChangesAsync();
    foreach (var l in pulled) { l.ReqId = h.Id; db.StockReqLines.Add(l); }
    foreach (var l in lines)
        db.StockReqLines.Add(new StockReqLine { OrgId = t.OrgId, ReqId = h.Id, PartCode = l.PartCode.Trim(), PartName = l.PartName, Location = l.Location, Quantity = l.Quantity <= 0 ? 1 : l.Quantity, Unit = l.Unit });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.ReqNo, h.RONo, lines = pulled.Count + lines.Count, status = h.Status });
}).RequireAuthorization();

app.MapGet("/api/stockreqs/{no}/lines", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.StockReqs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo.ToUpper() == no);
    if (h is null) return Results.NotFound(new { no });
    var lines = await db.StockReqLines.Where(l => l.OrgId == t.OrgId && l.ReqId == h.Id)
        .Select(l => new { l.PartCode, l.PartName, l.Location, l.Quantity, l.Unit }).ToListAsync();
    return Results.Ok(new { h.ReqNo, h.RONo, h.Status, count = lines.Count, lines });
}).RequireAuthorization();

app.MapPost("/api/stockreqs/{no}/issue", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.StockReqs.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.ReqNo.ToUpper() == no);
    if (h is null) return Results.NotFound(new { no });
    if (h.Status != "Draft") return Results.BadRequest(new { error = "Chỉ xuất được phiếu Draft." });
    h.Status = "Issued"; h.IssuedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { h.ReqNo, status = h.Status });
}).RequireAuthorization();

// ===== Lệnh sửa chữa RO (Ser_RO — port 1:1 FrmRepairOrder, TCMotor DMSCarSv) =====
string[] _roFlow = { "HasRO", "InGarage", "Repaired", "CheckEnd", "Paid", "Finished" };
app.MapGet("/api/repairorders", async (AppDbContext db, ITenantContext t, string? status, string? plate) =>
{
    var q = db.RepairOrders.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(r => r.Status == status);
    if (!string.IsNullOrWhiteSpace(plate)) q = q.Where(r => r.LicensePlate.Contains(plate.ToUpper()));
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    {
        r.RONo, r.LicensePlate, r.Vin, r.CusName, r.Km, r.CheckInDate, r.PlanedDeliveryDate, r.CusWaiting, r.Status,
        services = db.RoServiceItems.Count(s => s.OrgId == t.OrgId && s.RoId == r.Id),
        parts = db.RoPartItems.Count(p => p.OrgId == t.OrgId && p.RoId == r.Id),
        total = db.RoServiceItems.Where(s => s.OrgId == t.OrgId && s.RoId == r.Id).Sum(s => (decimal?)s.Amount) ?? 0
              + (db.RoPartItems.Where(p => p.OrgId == t.OrgId && p.RoId == r.Id).Sum(p => (decimal?)(p.NeedQty * p.UnitPrice)) ?? 0)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/repairorders", async (RepairOrderDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.LicensePlate)) return Results.BadRequest(new { error = "Cần biển số (LicensePlate)." });
    var no = "RO" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new RepairOrder
    {
        OrgId = t.OrgId, RONo = no, LicensePlate = dto.LicensePlate.Trim().ToUpperInvariant(), Vin = dto.Vin, CusName = dto.CusName, Km = dto.Km,
        CheckInDate = dto.CheckInDate ?? DateTime.Now, PlanedDeliveryDate = dto.PlanedDeliveryDate, CusRequest = dto.CusRequest,
        CarStatus = dto.CarStatus, CusWaiting = dto.CusWaiting, Status = "HasRO"
    };
    db.RepairOrders.Add(r); await db.SaveChangesAsync();
    foreach (var s in dto.Services ?? new())
        if (!string.IsNullOrWhiteSpace(s.SerCode))
            db.RoServiceItems.Add(new RoServiceItem { OrgId = t.OrgId, RoId = r.Id, SerCode = s.SerCode.Trim(), SerName = s.SerName, Cause = s.Cause, Engineer = s.Engineer, Amount = s.Amount });
    foreach (var p in dto.Parts ?? new())
        if (!string.IsNullOrWhiteSpace(p.PartCode))
            db.RoPartItems.Add(new RoPartItem { OrgId = t.OrgId, RoId = r.Id, PartCode = p.PartCode.Trim(), PartName = p.PartName, Unit = p.Unit, NeedQty = p.NeedQty <= 0 ? 1 : p.NeedQty, UnitPrice = p.UnitPrice, Note = p.Note });
    await db.SaveChangesAsync();
    return Results.Ok(new { r.RONo, r.LicensePlate, status = r.Status });
}).RequireAuthorization();

// Bảng theo dõi tiến độ RO (Ser_RO_Stage — port 1:1 FrmTrackingProcess): kanban theo trạng thái
app.MapGet("/api/repairorders/board", async (AppDbContext db, ITenantContext t) =>
{
    var flow = new[] { "HasRO", "InGarage", "Repaired", "CheckEnd", "Paid", "Finished" };
    var ros = await db.RepairOrders.Where(r => r.OrgId == t.OrgId).OrderByDescending(r => r.Id).Take(1000)
        .Select(r => new { r.RONo, r.LicensePlate, r.CusName, r.Km, r.PlanedDeliveryDate, r.CusWaiting, r.Status }).ToListAsync();
    var columns = flow.Select(st => new
    {
        status = st,
        count = ros.Count(r => r.Status == st),
        items = ros.Where(r => r.Status == st).Select(r => new { r.RONo, r.LicensePlate, r.CusName, r.Km, r.PlanedDeliveryDate, r.CusWaiting }).ToList()
    }).ToList();
    return Results.Ok(new { total = ros.Count, columns });
}).RequireAuthorization();

app.MapGet("/api/repairorders/{no}", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.RepairOrders.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.RONo == no);
    if (r is null) return Results.NotFound(new { no });
    var services = await db.RoServiceItems.Where(s => s.OrgId == t.OrgId && s.RoId == r.Id)
        .Select(s => new { s.SerCode, s.SerName, s.Cause, s.Result, s.Engineer, s.Amount }).ToListAsync();
    var parts = await db.RoPartItems.Where(p => p.OrgId == t.OrgId && p.RoId == r.Id)
        .Select(p => new { p.PartCode, p.PartName, p.Unit, p.NeedQty, p.UnitPrice, lineTotal = p.NeedQty * p.UnitPrice, p.Note }).ToListAsync();
    return Results.Ok(new
    {
        r.RONo, r.LicensePlate, r.Vin, r.CusName, r.Km, r.CheckInDate, r.PlanedDeliveryDate, r.CusRequest, r.CarStatus, r.CusWaiting, r.Status,
        services, parts,
        total = services.Sum(s => s.Amount) + parts.Sum(p => p.lineTotal)
    });
}).RequireAuthorization();

// Chuyển trạng thái theo đúng chuỗi Ser_RO_Stage
app.MapPost("/api/repairorders/{no}/advance", async (string no, RoAdvanceDto dto, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.RepairOrders.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.RONo == no);
    if (r is null) return Results.NotFound(new { no });
    var target = (dto.ToStatus ?? "").Trim();
    var curIdx = Array.IndexOf(_roFlow, r.Status);
    var tgtIdx = Array.IndexOf(_roFlow, target);
    if (tgtIdx < 0) return Results.BadRequest(new { error = "ToStatus không hợp lệ. Chuỗi: HasRO→InGarage→Repaired→CheckEnd→Paid→Finished" });
    if (tgtIdx != curIdx + 1) return Results.BadRequest(new { error = $"Chỉ tiến 1 bước từ {r.Status} sang {_roFlow[Math.Min(curIdx + 1, _roFlow.Length - 1)]}." });
    r.Status = target;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.RONo, status = r.Status });
}).RequireAuthorization();

// ===== Giá bán xe TCG theo spec (Mst_TCGCarSalePrice — port 1:1 FrmMstTCGCarSalePrice) =====
app.MapGet("/api/tcgsaleprices", async (AppDbContext db, ITenantContext t, string? spec) =>
{
    var q = db.TcgSalePrices.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(spec)) q = q.Where(p => p.SpecCode.Contains(spec.ToUpper()));
    var items = await q.OrderBy(p => p.SpecCode).Take(1000).Select(p => new { p.SpecCode, p.UnitPrice, p.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/tcgsaleprices", async (TcgPriceDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SpecCode)) return Results.BadRequest(new { error = "Cần SpecCode." });
    var spec = dto.SpecCode.Trim().ToUpperInvariant();
    var p = await db.TcgSalePrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SpecCode == spec);
    if (p is null) { p = new TcgSalePrice { OrgId = t.OrgId, SpecCode = spec }; db.TcgSalePrices.Add(p); }
    p.UnitPrice = dto.UnitPrice; p.Status = dto.Status ?? "1"; p.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.SpecCode, p.UnitPrice, p.Status });
}).RequireAuthorization();

// Điều chỉnh hạn mức (FrmAdjustQuota — bản WinForm là STUB rỗng; implement thật: cộng/trừ delta vào Qty của Quota)
app.MapPost("/api/quotas/adjust", async (QuotaAdjustDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.ModelCode) || string.IsNullOrWhiteSpace(dto.Period))
        return Results.BadRequest(new { error = "Cần DealerCode, ModelCode và Period." });
    var q = await db.Quotas.FirstOrDefaultAsync(x => x.OrgId == t.OrgId
        && x.DealerCode == dto.DealerCode.Trim().ToUpperInvariant() && x.ModelCode == dto.ModelCode.Trim().ToUpperInvariant() && x.Period == dto.Period.Trim());
    if (q is null) return Results.NotFound(new { error = "Chưa có hạn mức để điều chỉnh." });
    var newQty = q.Qty + dto.DeltaQty;
    if (newQty < q.UsedQty) return Results.BadRequest(new { error = $"Hạn mức mới ({newQty}) < đã dùng ({q.UsedQty})." });
    q.Qty = newQty; q.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { q.DealerCode, q.ModelCode, q.Period, q.Qty, q.UsedQty, remain = q.Qty - q.UsedQty });
}).RequireAuthorization();

// ===== Giá thiết bị theo spec (Mst_DevicePrice_Spec — port 1:1 FrmMst_DevicePrice_Spec) =====
app.MapGet("/api/deviceprices", async (AppDbContext db, ITenantContext t, string? spec, string? device) =>
{
    var q = db.DevicePrices.Where(d => d.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(spec)) q = q.Where(d => d.SpecCode == spec);
    if (!string.IsNullOrWhiteSpace(device)) q = q.Where(d => d.DeviceCode.Contains(device.ToUpper()));
    var items = await q.OrderBy(d => d.SpecCode).ThenBy(d => d.DeviceCode).Take(1000).Select(d => new
    { d.SpecCode, d.SpecDescription, d.DeviceTypeCode, d.DeviceCode, d.DeviceName, d.Price, d.VAT, d.PriceVAT, d.EffectiveDate, d.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/deviceprices", async (DevicePriceDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SpecCode) || string.IsNullOrWhiteSpace(dto.DeviceCode))
        return Results.BadRequest(new { error = "Cần SpecCode và DeviceCode." });
    var spec = dto.SpecCode.Trim().ToUpperInvariant();
    var dev = dto.DeviceCode.Trim().ToUpperInvariant();
    var ed = dto.EffectiveDate?.Date;
    // upsert theo (spec + device + ngày hiệu lực)
    var d = await db.DevicePrices.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SpecCode == spec && x.DeviceCode == dev && x.EffectiveDate == ed);
    if (d is null) { d = new DevicePrice { OrgId = t.OrgId, SpecCode = spec, DeviceCode = dev, EffectiveDate = ed }; db.DevicePrices.Add(d); }
    d.SpecDescription = dto.SpecDescription; d.DeviceTypeCode = dto.DeviceTypeCode; d.DeviceName = dto.DeviceName;
    d.Price = dto.Price; d.VAT = dto.VAT; d.PriceVAT = Math.Round(dto.Price * (1 + dto.VAT / 100m), 2);   // tính PriceVAT
    d.Status = dto.Status ?? "1"; d.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { d.SpecCode, d.DeviceCode, d.Price, d.VAT, d.PriceVAT });
}).RequireAuthorization();

// ===== Biểu chiết khấu/phạt theo ngày hiệu lực (Mst_Discount — port 1:1 FrmDiscount) =====
app.MapGet("/api/discounts", async (AppDbContext db, ITenantContext t, string? onDate) =>
{
    var q = db.Discounts.Where(d => d.OrgId == t.OrgId);
    var items = await q.OrderByDescending(d => d.EffectiveDate).Take(500).Select(d => new
    { d.EffectiveDate, d.DiscountPercent, d.PenaltyPercent, d.PenaltyPercentTCKT, d.FnExpPercent, d.PmtDsTCGPercent, d.Status }).ToListAsync();
    // biểu áp dụng cho 1 ngày = bản hiệu lực mới nhất ≤ ngày đó
    object? applicable = null;
    if (DateTime.TryParse(onDate, out var od))
        applicable = items.Where(x => x.EffectiveDate <= od).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
    return Results.Ok(new { count = items.Count, applicable, items });
}).RequireAuthorization();

app.MapPost("/api/discounts", async (DiscountDto dto, AppDbContext db, ITenantContext t) =>
{
    if (dto.EffectiveDate is null) return Results.BadRequest(new { error = "Cần EffectiveDate." });
    var ed = dto.EffectiveDate.Value.Date;
    var d = await db.Discounts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.EffectiveDate == ed);
    if (d is null) { d = new Discount { OrgId = t.OrgId, EffectiveDate = ed }; db.Discounts.Add(d); }
    d.DiscountPercent = dto.DiscountPercent; d.PenaltyPercent = dto.PenaltyPercent; d.PenaltyPercentTCKT = dto.PenaltyPercentTCKT;
    d.FnExpPercent = dto.FnExpPercent; d.PmtDsTCGPercent = dto.PmtDsTCGPercent; d.Status = dto.Status ?? "1"; d.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { d.EffectiveDate, d.DiscountPercent, d.PenaltyPercent });
}).RequireAuthorization();

// ===== Xe kho bảo dưỡng gia hạn (StoF_MaintainMain — port 1:1 FrmMaintenanceWarehouse) =====
app.MapGet("/api/maintext", async (AppDbContext db, ITenantContext t, string? status, string? storage) =>
{
    var q = db.MaintainExts.Where(m => m.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(m => m.MtnExtStatusMain == status);
    if (!string.IsNullOrWhiteSpace(storage)) q = q.Where(m => m.StorageCode == storage);
    var items = await q.OrderByDescending(m => m.Id).Take(500).Select(m => new
    { m.Vin, m.ModelCode, m.StorageCode, m.MtnExtStartDTime, m.MtnExtEndDTime, m.MtnExtRemark, m.MtnExtStatusMain }).ToListAsync();
    return Results.Ok(new
    {
        count = items.Count,
        inProgress = items.Count(x => x.MtnExtStatusMain == "IN"),
        done = items.Count(x => x.MtnExtStatusMain == "OUT"),
        items
    });
}).RequireAuthorization();

app.MapPost("/api/maintext", async (MaintExtDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var vin = dto.Vin.Trim().ToUpperInvariant();
    var m = await db.MaintainExts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Vin == vin);
    if (m is null) { m = new MaintainExt { OrgId = t.OrgId, Vin = vin, MtnExtStatusMain = "NG" }; db.MaintainExts.Add(m); }
    m.ModelCode = dto.ModelCode; m.StorageCode = dto.StorageCode; m.MtnExtRemark = dto.MtnExtRemark; m.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { m.Vin, status = m.MtnExtStatusMain });
}).RequireAuthorization();

// Vào (MtnExtIn) / Ra (MtnExtOut) bảo dưỡng gia hạn
app.MapPost("/api/maintext/{vin}/{action}", async (string vin, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("in" or "out")) return Results.BadRequest(new { error = "action = in|out" });
    vin = vin.Trim().ToUpperInvariant();
    var m = await db.MaintainExts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Vin == vin);
    if (m is null) return Results.NotFound(new { vin });
    if (action == "in")
    {
        if (m.MtnExtStatusMain == "IN") return Results.BadRequest(new { error = "Xe đang trong BD gia hạn." });
        m.MtnExtStatusMain = "IN"; m.MtnExtStartDTime = DateTime.Now; m.MtnExtEndDTime = null;
    }
    else // out
    {
        if (m.MtnExtStatusMain != "IN") return Results.BadRequest(new { error = "Xe chưa vào BD gia hạn (IN)." });
        m.MtnExtStatusMain = "OUT"; m.MtnExtEndDTime = DateTime.Now;
    }
    m.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { m.Vin, status = m.MtnExtStatusMain, m.MtnExtStartDTime, m.MtnExtEndDTime });
}).RequireAuthorization();

// ===== Bảo dưỡng xe tồn kho theo kỳ (VIN_MaintainPeriodHist — port 1:1 FrmMaintenanceHistory) =====
app.MapGet("/api/carmaintenances", async (AppDbContext db, ITenantContext t, string? vin, string? type, string? storage) =>
{
    var q = db.CarMaintenances.Where(m => m.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(vin)) q = q.Where(m => m.Vin.Contains(vin.ToUpper()));
    if (!string.IsNullOrWhiteSpace(type)) q = q.Where(m => m.MtnType == type);
    if (!string.IsNullOrWhiteSpace(storage)) q = q.Where(m => m.StorageCode == storage);
    var items = await q.OrderByDescending(m => m.Id).Take(1000).Select(m => new
    { m.Vin, m.StorageCode, m.ModelCode, m.MtnType, m.MtnTimes, m.MtnDate, m.MtnNextDate, m.UserCode, m.Remark }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

// Ghi 1 lần bảo dưỡng; MtnTimes = lần thứ n theo VIN+loại; MtnNextDate = MtnDate + chu kỳ (mặc định 90 ngày)
app.MapPost("/api/carmaintenances", async (CarMtnDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Vin)) return Results.BadRequest(new { error = "Cần Vin." });
    var type = string.IsNullOrWhiteSpace(dto.MtnType) ? "MAINTAINANCE" : dto.MtnType.Trim().ToUpperInvariant();
    if (type is not ("MAINTAINANCE" or "EXT")) return Results.BadRequest(new { error = "MtnType = MAINTAINANCE|EXT" });
    var vin = dto.Vin.Trim().ToUpperInvariant();
    var lastTimes = await db.CarMaintenances.Where(m => m.OrgId == t.OrgId && m.Vin == vin && m.MtnType == type)
        .Select(m => (int?)m.MtnTimes).MaxAsync() ?? 0;
    var mtnDate = dto.MtnDate ?? DateTime.Now;
    int cycle = dto.CycleDays is int c && c > 0 ? c : 90;
    var m = new CarMaintenance
    {
        OrgId = t.OrgId, Vin = vin, StorageCode = dto.StorageCode, ModelCode = dto.ModelCode, MtnType = type,
        MtnTimes = lastTimes + 1, MtnDate = mtnDate, MtnNextDate = mtnDate.AddDays(cycle), UserCode = dto.UserCode, Remark = dto.Remark
    };
    db.CarMaintenances.Add(m); await db.SaveChangesAsync();
    return Results.Ok(new { m.Vin, m.MtnType, m.MtnTimes, m.MtnDate, m.MtnNextDate });
}).RequireAuthorization();

// ===== NVBH đại lý (Mst_DlSalesMan — port 1:1 FrmMngSalesManHTC/FrmMngSalesManApproved) =====
string[] _smStatuses = { "THUVIEC", "CHINGTHUC", "CTVIEN", "NGHIVIEC" };
app.MapGet("/api/dlsalesmen", async (AppDbContext db, ITenantContext t, string? dealer, string? status, string? approved) =>
{
    var q = db.DlSalesMen.Where(s => s.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(s => s.DealerCode == dealer);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(s => s.SMStatus == status);
    if (approved == "1") q = q.Where(s => s.SMHyundaiCode != null && s.SMHyundaiCode != "");
    else if (approved == "0") q = q.Where(s => s.SMHyundaiCode == null || s.SMHyundaiCode == "");
    var items = await q.OrderBy(s => s.SMCode).Take(500).Select(s => new
    { s.SMCode, s.SMName, s.DealerCode, s.SMHyundaiCode, s.SMStatus, s.Sex, s.DateOfBirth, s.PhoneNo, s.IdentityCardNo }).ToListAsync();
    return Results.Ok(new { count = items.Count, approved = items.Count(x => !string.IsNullOrEmpty(x.SMHyundaiCode)), items });
}).RequireAuthorization();

app.MapPost("/api/dlsalesmen", async (DlSalesManDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SMCode) || string.IsNullOrWhiteSpace(dto.SMName))
        return Results.BadRequest(new { error = "Cần SMCode và SMName." });
    var code = dto.SMCode.Trim().ToUpperInvariant();
    var s = await db.DlSalesMen.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SMCode == code);
    if (s is null) { s = new DlSalesMan { OrgId = t.OrgId, SMCode = code }; db.DlSalesMen.Add(s); }
    s.SMName = dto.SMName; s.DealerCode = (dto.DealerCode ?? "").Trim().ToUpperInvariant();
    s.Sex = dto.Sex; s.DateOfBirth = dto.DateOfBirth; s.PhoneNo = dto.PhoneNo; s.IdentityCardNo = dto.IdentityCardNo;
    if (!string.IsNullOrWhiteSpace(dto.SMStatus))
    {
        var st = dto.SMStatus.Trim().ToUpperInvariant();
        if (!_smStatuses.Contains(st)) return Results.BadRequest(new { error = "SMStatus = THUVIEC|CHINGTHUC|CTVIEN|NGHIVIEC" });
        s.SMStatus = st;
    }
    s.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { s.SMCode, s.SMName, s.SMStatus, s.SMHyundaiCode });
}).RequireAuthorization();

// Duyệt = cấp mã Hyundai (FrmMngSalesManApproved)
app.MapPost("/api/dlsalesmen/{code}/grant", async (string code, DlGrantDto dto, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var s = await db.DlSalesMen.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SMCode == code);
    if (s is null) return Results.NotFound(new { code });
    if (string.IsNullOrWhiteSpace(dto.SMHyundaiCode)) return Results.BadRequest(new { error = "Cần SMHyundaiCode để duyệt." });
    if (!string.IsNullOrEmpty(s.SMHyundaiCode)) return Results.BadRequest(new { error = $"NV đã có mã Hyundai {s.SMHyundaiCode}." });
    s.SMHyundaiCode = dto.SMHyundaiCode.Trim().ToUpperInvariant();
    if (s.SMStatus == "THUVIEC") s.SMStatus = "CHINGTHUC";   // duyệt → chuyển chính thức
    s.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { s.SMCode, s.SMHyundaiCode, s.SMStatus });
}).RequireAuthorization();

// Đổi trạng thái làm việc (vd nghỉ việc)
app.MapPost("/api/dlsalesmen/{code}/status", async (string code, DlStatusDto dto, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var s = await db.DlSalesMen.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SMCode == code);
    if (s is null) return Results.NotFound(new { code });
    var st = (dto.SMStatus ?? "").Trim().ToUpperInvariant();
    if (!_smStatuses.Contains(st)) return Results.BadRequest(new { error = "SMStatus = THUVIEC|CHINGTHUC|CTVIEN|NGHIVIEC" });
    s.SMStatus = st; s.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { s.SMCode, s.SMStatus });
}).RequireAuthorization();

// ===== Vi phạm NVBH (HR_SalesManViolate — port 1:1 FrmCreateSalesManViolate/FrmMngSalesManViolate) =====
app.MapGet("/api/smviolates", async (AppDbContext db, ITenantContext t, string? salesman, string? dealer, string? type) =>
{
    var q = db.SalesManViolates.Where(v => v.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(salesman)) q = q.Where(v => v.SalesManCode == salesman);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(v => v.DealerCode == dealer);
    if (!string.IsNullOrWhiteSpace(type)) q = q.Where(v => v.ViolateTypeId == type);
    var items = await q.OrderByDescending(v => v.Id).Take(500).Select(v => new
    { v.SalesManCode, v.SalesManName, v.DealerCode, v.ViolateTypeId, v.ViolateNumber, v.ViolateDateStart, v.ViolateDateEnd, v.Remark, v.CreatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/smviolates", async (SmViolateDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.SalesManCode) || string.IsNullOrWhiteSpace(dto.ViolateTypeId))
        return Results.BadRequest(new { error = "Cần SalesManCode và ViolateTypeId." });
    var sm = dto.SalesManCode.Trim().ToUpperInvariant();
    // ViolateNumber = lần vi phạm thứ n của NV này (auto +1 như FrmCreateSalesManViolate)
    var lastNo = await db.SalesManViolates.Where(v => v.OrgId == t.OrgId && v.SalesManCode == sm)
        .Select(v => (int?)v.ViolateNumber).MaxAsync() ?? 0;
    var v = new SalesManViolate
    {
        OrgId = t.OrgId, SalesManCode = sm, SalesManName = dto.SalesManName, DealerCode = (dto.DealerCode ?? "").Trim().ToUpperInvariant(),
        ViolateTypeId = dto.ViolateTypeId.Trim().ToUpperInvariant(), ViolateNumber = lastNo + 1,
        ViolateDateStart = dto.ViolateDateStart, ViolateDateEnd = dto.ViolateDateEnd,
        IdentityCardNo = dto.IdentityCardNo, PhoneNo = dto.PhoneNo, Remark = dto.Remark
    };
    db.SalesManViolates.Add(v); await db.SaveChangesAsync();
    return Results.Ok(new { v.SalesManCode, v.ViolateTypeId, v.ViolateNumber });
}).RequireAuthorization();

// ===== Tồn/gán thiết bị GPS ↔ VIN (Sto_StoBalanceGPS — port 1:1 FrmMngSto_StoBalanceGPS + FrmUnmapThietBi) =====
app.MapGet("/api/gpsbalance", async (AppDbContext db, ITenantContext t, string? status, string? dealer, string? vin, string? device) =>
{
    var q = db.GpsBalances.Where(g => g.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(g => g.Status == status);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(g => g.DealerCode == dealer);
    if (!string.IsNullOrWhiteSpace(vin)) q = q.Where(g => g.Vin != null && g.Vin.Contains(vin.ToUpper()));
    if (!string.IsNullOrWhiteSpace(device)) q = q.Where(g => g.GpsDvNo.Contains(device.ToUpper()));
    var items = await q.OrderByDescending(g => g.Id).Take(1000).Select(g => new
    { g.GpsDvNo, g.Vin, g.DealerCode, g.DealerName, g.Address, g.StorageCode, g.MapVINDateTime, g.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, mapped = items.Count(x => x.Status == "Mapped"), items });
}).RequireAuthorization();

// Gắn thiết bị GPS lên VIN
app.MapPost("/api/gpsbalance/map", async (GpsMapDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.GpsDvNo) || string.IsNullOrWhiteSpace(dto.Vin))
        return Results.BadRequest(new { error = "Cần GpsDvNo và Vin." });
    var dv = dto.GpsDvNo.Trim().ToUpperInvariant();
    var vin = dto.Vin.Trim().ToUpperInvariant();
    // 1 VIN chỉ gắn 1 thiết bị đang hoạt động; 1 thiết bị chỉ gắn 1 VIN
    var vinBusy = await db.GpsBalances.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Status == "Mapped" && x.Vin == vin && x.GpsDvNo != dv);
    if (vinBusy is not null) return Results.BadRequest(new { error = $"VIN {vin} đã gắn thiết bị {vinBusy.GpsDvNo}." });
    var g = await db.GpsBalances.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GpsDvNo == dv);
    if (g is null) { g = new GpsBalance { OrgId = t.OrgId, GpsDvNo = dv }; db.GpsBalances.Add(g); }
    g.Vin = vin; g.DealerCode = dto.DealerCode; g.DealerName = dto.DealerName; g.Address = dto.Address; g.StorageCode = dto.StorageCode;
    g.MapVINDateTime = DateTime.Now; g.Status = "Mapped";
    // ghi lịch sử (Sto_StoTransactionGPS): mở 1 giao dịch gắn mới
    db.GpsTransactions.Add(new GpsTransaction { OrgId = t.OrgId, Vin = vin, GpsDvNo = dv, VINAddress = dto.Address, MapDateTime = DateTime.Now });
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GpsDvNo, g.Vin, status = g.Status });
}).RequireAuthorization();

// Gỡ map thiết bị (FrmUnmapThietBi)
app.MapPost("/api/gpsbalance/{device}/unmap", async (string device, AppDbContext db, ITenantContext t) =>
{
    device = device.Trim().ToUpperInvariant();
    var g = await db.GpsBalances.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GpsDvNo == device);
    if (g is null) return Results.NotFound(new { device });
    if (g.Status != "Mapped") return Results.BadRequest(new { error = "Thiết bị chưa gắn VIN." });
    // đóng giao dịch gắn đang mở (set UnMapDateTime) trước khi xoá VIN khỏi balance
    var openTx = await db.GpsTransactions.Where(x => x.OrgId == t.OrgId && x.GpsDvNo == device && x.Vin == g.Vin && x.UnMapDateTime == null)
        .OrderByDescending(x => x.Id).FirstOrDefaultAsync();
    if (openTx is not null) openTx.UnMapDateTime = DateTime.Now;
    g.Status = "Unmapped"; g.Vin = null; g.MapVINDateTime = null;
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GpsDvNo, status = g.Status });
}).RequireAuthorization();

// ===== Lịch sử gắn/gỡ GPS theo VIN (Sto_StoTransactionGPS — port 1:1 FrmMngVinHistoryMap) =====
app.MapGet("/api/gpshistory", async (AppDbContext db, ITenantContext t, string? vin, string? device, string? open) =>
{
    var q = db.GpsTransactions.Where(x => x.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(vin)) q = q.Where(x => x.Vin.Contains(vin.ToUpper()));
    if (!string.IsNullOrWhiteSpace(device)) q = q.Where(x => x.GpsDvNo.Contains(device.ToUpper()));
    if (open == "1") q = q.Where(x => x.UnMapDateTime == null);
    var items = await q.OrderByDescending(x => x.Id).Take(1000)
        .Select(x => new { x.Vin, x.GpsDvNo, x.VINAddress, x.MapDateTime, x.UnMapDateTime }).ToListAsync();
    return Results.Ok(new { count = items.Count, active = items.Count(i => i.UnMapDateTime == null), items });
}).RequireAuthorization();

// ===== Phiếu xuất kho thiết bị GPS (StoF_GPSOut — port 1:1 FrmStoF_GPSOut/FrmMngStoF_GPSOut) =====
app.MapGet("/api/gpsouts", async (AppDbContext db, ITenantContext t, string? storage) =>
{
    var q = db.GpsOuts.Where(g => g.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(storage)) q = q.Where(g => g.StorageCode == storage);
    var items = await q.OrderByDescending(g => g.Id).Take(500).Select(g => new
    {
        g.SFGPSOutNo, g.StorageCode, g.UserCodeReceived, g.Remark, g.CreatedAt,
        devices = db.GpsOutDetails.Count(d => d.OrgId == t.OrgId && d.OutId == g.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/gpsouts", async (GpsOutDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.StorageCode)) return Results.BadRequest(new { error = "Cần StorageCode (kho GPS)." });
    var devs = (dto.Devices ?? new List<GpsInDevDto>()).Where(d => !string.IsNullOrWhiteSpace(d.GpsDvNo)).ToList();
    if (devs.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 thiết bị GPS." });
    var dupe = devs.GroupBy(d => d.GpsDvNo.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"Thiết bị {dupe.Key} bị trùng!" });
    var no = "GPSOUT" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new GpsOut { OrgId = t.OrgId, SFGPSOutNo = no, StorageCode = dto.StorageCode.Trim().ToUpperInvariant(), UserCodeReceived = dto.UserCodeReceived, Remark = dto.Remark };
    db.GpsOuts.Add(h); await db.SaveChangesAsync();
    foreach (var d in devs)
    {
        db.GpsOutDetails.Add(new GpsOutDetail { OrgId = t.OrgId, OutId = h.Id, GpsDvNo = d.GpsDvNo.Trim().ToUpperInvariant(), GpsBoxNo = d.GpsBoxNo, MapStatus = "1", Remark = d.Remark });
        // xuất kho = gắn lên xe → đánh dấu MapStatus='1' trên tồn nhập nếu có
        var inDtl = await db.GpsInDetails.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GpsDvNo == d.GpsDvNo.Trim().ToUpperInvariant());
        if (inDtl is not null) inDtl.MapStatus = "1";
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { h.SFGPSOutNo, h.StorageCode, devices = devs.Count });
}).RequireAuthorization();

app.MapGet("/api/gpsouts/{no}/devices", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.GpsOuts.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SFGPSOutNo == no);
    if (h is null) return Results.NotFound(new { no });
    var devices = await db.GpsOutDetails.Where(d => d.OrgId == t.OrgId && d.OutId == h.Id)
        .Select(d => new { d.GpsDvNo, d.GpsBoxNo, d.MapStatus, d.Remark }).ToListAsync();
    return Results.Ok(new { h.SFGPSOutNo, h.StorageCode, count = devices.Count, devices });
}).RequireAuthorization();

// ===== Địa điểm nhận xe của đại lý (Mst_PointRegis — port 1:1 FrmMst_PointRegis) =====
app.MapGet("/api/pointregis", async (AppDbContext db, ITenantContext t, string? dealer) =>
{
    var q = db.PointRegises.Where(p => p.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(p => p.DealerCode == dealer);
    var items = await q.OrderBy(p => p.PointRegisCode).Take(1000)
        .Select(p => new { p.PointRegisCode, p.DealerCode, p.PointRegisName, p.MapLatitude, p.MapLongitude, p.Radius }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/pointregis", async (PointRegisDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.PointRegisCode) || string.IsNullOrWhiteSpace(dto.DealerCode))
        return Results.BadRequest(new { error = "Cần PointRegisCode và DealerCode." });
    var code = dto.PointRegisCode.Trim().ToUpperInvariant();
    var p = await db.PointRegises.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.PointRegisCode == code);
    if (p is null) { p = new PointRegis { OrgId = t.OrgId, PointRegisCode = code }; db.PointRegises.Add(p); }
    p.DealerCode = dto.DealerCode.Trim().ToUpperInvariant(); p.PointRegisName = dto.PointRegisName ?? "";
    p.MapLatitude = dto.MapLatitude; p.MapLongitude = dto.MapLongitude; p.Radius = dto.Radius; p.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { p.PointRegisCode, p.DealerCode, p.MapLatitude, p.MapLongitude, p.Radius });
}).RequireAuthorization();

// ===== Phiếu nhập kho thiết bị GPS (StoF_GPSIn — port 1:1 FrmStoF_GPSIn/FrmMngStoF_GPSIn) =====
app.MapGet("/api/gpsins", async (AppDbContext db, ITenantContext t, string? storage) =>
{
    var q = db.GpsIns.Where(g => g.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(storage)) q = q.Where(g => g.StorageCode == storage);
    var items = await q.OrderByDescending(g => g.Id).Take(500).Select(g => new
    {
        g.SFGPSInNo, g.GpsInType, g.StorageCode, g.Remark, g.CreatedAt,
        devices = db.GpsInDetails.Count(d => d.OrgId == t.OrgId && d.InId == g.Id),
        mapped = db.GpsInDetails.Count(d => d.OrgId == t.OrgId && d.InId == g.Id && d.MapStatus == "1")
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/gpsins", async (GpsInDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.StorageCode)) return Results.BadRequest(new { error = "Cần StorageCode (kho GPS)." });
    var devs = (dto.Devices ?? new List<GpsInDevDto>()).Where(d => !string.IsNullOrWhiteSpace(d.GpsDvNo)).ToList();
    if (devs.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 thiết bị GPS." });
    var dupe = devs.GroupBy(d => d.GpsDvNo.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"Thiết bị {dupe.Key} bị trùng!" });
    var no = "GPSIN" + DateTime.Now.ToString("yyMMddHHmmss");
    var h = new GpsIn { OrgId = t.OrgId, SFGPSInNo = no, GpsInType = dto.GpsInType, StorageCode = dto.StorageCode.Trim().ToUpperInvariant(), Remark = dto.Remark };
    db.GpsIns.Add(h); await db.SaveChangesAsync();
    foreach (var d in devs)
        db.GpsInDetails.Add(new GpsInDetail { OrgId = t.OrgId, InId = h.Id, GpsDvNo = d.GpsDvNo.Trim().ToUpperInvariant(), GpsBoxNo = d.GpsBoxNo, MapStatus = "0", Remark = d.Remark });
    await db.SaveChangesAsync();
    return Results.Ok(new { h.SFGPSInNo, h.StorageCode, devices = devs.Count });
}).RequireAuthorization();

app.MapGet("/api/gpsins/{no}/devices", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var h = await db.GpsIns.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.SFGPSInNo == no);
    if (h is null) return Results.NotFound(new { no });
    var devices = await db.GpsInDetails.Where(d => d.OrgId == t.OrgId && d.InId == h.Id)
        .Select(d => new { d.GpsDvNo, d.GpsBoxNo, d.MapStatus, d.Remark }).ToListAsync();
    return Results.Ok(new { h.SFGPSInNo, h.StorageCode, count = devices.Count, devices });
}).RequireAuthorization();

// ===== Yêu cầu sửa/bảo hành thiết bị GPS (GPSF_GPSClaim — port 1:1 FrmGPSF_GPSClaimNew/FrmGPSF_GPSClaimMng) =====
app.MapGet("/api/gpsclaims", async (AppDbContext db, ITenantContext t, string? claimStatus, string? device) =>
{
    var q = db.GpsClaims.Where(g => g.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(claimStatus)) q = q.Where(g => g.ClaimStatus == claimStatus);
    if (!string.IsNullOrWhiteSpace(device)) q = q.Where(g => g.GpsDvNo.Contains(device.ToUpper()));
    var items = await q.OrderByDescending(g => g.Id).Take(500)
        .Select(g => new { g.GpsClaimNo, g.GpsDvNo, g.BeforeFixRemark, g.Remark, g.ClaimStatus, g.ReceivedStatus, g.FixStatus, g.CreatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/gpsclaims", async (GpsClaimDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.GpsDvNo)) return Results.BadRequest(new { error = "Cần số thiết bị GPS (GpsDvNo)." });
    var no = "GPSC" + DateTime.Now.ToString("yyMMddHHmmss");
    var g = new GpsClaim { OrgId = t.OrgId, GpsClaimNo = no, GpsDvNo = dto.GpsDvNo.Trim().ToUpperInvariant(), BeforeFixRemark = dto.BeforeFixRemark, Remark = dto.Remark, ClaimStatus = "Pending" };
    db.GpsClaims.Add(g); await db.SaveChangesAsync();
    return Results.Ok(new { g.GpsClaimNo, g.GpsDvNo, claimStatus = g.ClaimStatus });
}).RequireAuthorization();

// Chuyển trạng thái: approve (Claim→Approved), receive (Received→Progress), finish (Received→Finished + Fix→Finished)
app.MapPost("/api/gpsclaims/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "receive" or "finish")) return Results.BadRequest(new { error = "action = approve|receive|finish" });
    no = no.Trim().ToUpperInvariant();
    var g = await db.GpsClaims.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.GpsClaimNo == no);
    if (g is null) return Results.NotFound(new { no });
    if (action == "approve")
    {
        if (g.ClaimStatus != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt claim Mới tạo." });
        g.ClaimStatus = "Approved"; g.ApprovedAt = DateTime.Now;
    }
    else if (action == "receive")
    {
        if (g.ClaimStatus != "Approved") return Results.BadRequest(new { error = "Chưa duyệt claim." });
        if (g.ReceivedStatus == "Finished") return Results.BadRequest(new { error = "Đã hoàn tất." });
        g.ReceivedStatus = "Progress";
    }
    else // finish
    {
        if (g.ReceivedStatus != "Progress") return Results.BadRequest(new { error = "Chưa nhận thiết bị (Progress)." });
        g.ReceivedStatus = "Finished"; g.FixStatus = "Finished";
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { g.GpsClaimNo, g.ClaimStatus, g.ReceivedStatus, g.FixStatus });
}).RequireAuthorization();

// ===== Cập nhật trạng thái đóng thùng (CarVINUpdate_TypeCB — port 1:1 FrmUpdateVIN_TypeCB) =====
app.MapGet("/api/vinpackings", async (AppDbContext db, ITenantContext t, string? vin, string? typeCB) =>
{
    var q = db.VinPackings.Where(v => v.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(vin)) q = q.Where(v => v.Vin.Contains(vin.ToUpper()));
    if (!string.IsNullOrWhiteSpace(typeCB)) q = q.Where(v => v.TypeCB == typeCB);
    var items = await q.OrderByDescending(v => v.Id).Take(500)
        .Select(v => new { v.Vin, v.TypeCB, v.LoaiThung, v.ActualSpec, v.SerialNo, v.InspectionDate, v.UpdatedAt }).ToListAsync();
    return Results.Ok(new { count = items.Count, packed = items.Count(x => x.TypeCB == "1"), items });
}).RequireAuthorization();

// Batch upsert như import Excel: mỗi VIN → set TypeCB='1' (đã đóng thùng) + LoaiThung/ActualSpec/SerialNo/InspectionDate
app.MapPost("/api/vinpackings", async (VinPackingDto dto, AppDbContext db, ITenantContext t) =>
{
    var rows = (dto.Items ?? new List<VinPackingRowDto>()).Where(r => !string.IsNullOrWhiteSpace(r.Vin)).ToList();
    if (rows.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    int updated = 0;
    foreach (var r in rows)
    {
        var vin = r.Vin.Trim().ToUpperInvariant();
        var v = await db.VinPackings.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.Vin == vin);
        if (v is null) { v = new VinPacking { OrgId = t.OrgId, Vin = vin }; db.VinPackings.Add(v); }
        v.TypeCB = "1"; v.LoaiThung = r.LoaiThung; v.ActualSpec = r.ActualSpec; v.SerialNo = r.SerialNo;
        v.InspectionDate = r.InspectionDate; v.UpdatedAt = DateTime.Now;
        updated++;
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { updated });
}).RequireAuthorization();

// ===== Cập nhật VIN thật FVIN→RVIN (Sto_TranspPlanMapVinReal — port 1:1 FrmUpdateFVINToRVIN) =====
// Map VIN kế hoạch (FVIN) sang VIN thật (RVIN) trên KH vận chuyển; batch như import Excel A1.
app.MapPost("/api/transplans/mapvin", async (MapVinDto dto, AppDbContext db, ITenantContext t) =>
{
    var pairs = (dto.Pairs ?? new List<VinPairDto>())
        .Where(p => !string.IsNullOrWhiteSpace(p.FVIN) && !string.IsNullOrWhiteSpace(p.RVIN)).ToList();
    if (pairs.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 cặp FVIN/RVIN." });
    var results = new List<object>();
    int mapped = 0;
    foreach (var p in pairs)
    {
        var fv = p.FVIN.Trim().ToUpperInvariant();
        var rv = p.RVIN.Trim().ToUpperInvariant();
        var plan = await db.TransportPlans.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.VINPlan == fv);
        if (plan is null) { results.Add(new { fvin = fv, rvin = rv, ok = false, warning = $"VIN kế hoạch {fv} không tồn tại (không map được carId)." }); continue; }
        plan.Vin = rv; mapped++;
        results.Add(new { fvin = fv, rvin = rv, ok = true });
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { total = pairs.Count, mapped, results });
}).RequireAuthorization();

// ===== Yêu cầu vận chuyển thu hồi xe (StoTranspReq — port 1:1 FrmNewRetrieveTransReq/FrmMngRetrieveTransReq) =====
app.MapGet("/api/retrievereqs", async (AppDbContext db, ITenantContext t, string? status, string? dealer) =>
{
    var q = db.RetrieveRequests.Where(r => r.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(status)) q = q.Where(r => r.Status == status);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(r => r.DealerCode == dealer);
    var items = await q.OrderByDescending(r => r.Id).Take(500).Select(r => new
    {
        r.TranspReqNo, r.DealerCode, r.TransporterCode, r.Reason, r.Status, r.CreatedAt, r.DecidedAt,
        cars = db.RetrieveReqCars.Count(c => c.OrgId == t.OrgId && c.ReqId == r.Id)
    }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/retrievereqs", async (RetrieveReqDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.TransporterCode))
        return Results.BadRequest(new { error = "Cần DealerCode và TransporterCode." });
    var vins = (dto.Cars ?? new List<RetrieveReqCarDto>()).Where(c => !string.IsNullOrWhiteSpace(c.Vin)).ToList();
    if (vins.Count == 0) return Results.BadRequest(new { error = "Cần ít nhất 1 VIN." });
    var dupe = vins.GroupBy(c => c.Vin.Trim().ToUpperInvariant()).FirstOrDefault(g => g.Count() > 1);
    if (dupe != null) return Results.BadRequest(new { error = $"VIN {dupe.Key} bị trùng!" });
    var no = "RTR" + DateTime.Now.ToString("yyMMddHHmmss");
    var r = new RetrieveRequest { OrgId = t.OrgId, TranspReqNo = no, DealerCode = dto.DealerCode.Trim().ToUpperInvariant(), TransporterCode = dto.TransporterCode.Trim().ToUpperInvariant(), Reason = dto.Reason, Status = "Pending" };
    db.RetrieveRequests.Add(r); await db.SaveChangesAsync();
    foreach (var c in vins)
        db.RetrieveReqCars.Add(new RetrieveReqCar { OrgId = t.OrgId, ReqId = r.Id, Vin = c.Vin.Trim().ToUpperInvariant(), StorageCode = c.StorageCode, DtlStatus = "Pending" });
    await db.SaveChangesAsync();
    return Results.Ok(new { r.TranspReqNo, r.DealerCode, r.TransporterCode, cars = vins.Count, status = r.Status });
}).RequireAuthorization();

app.MapGet("/api/retrievereqs/{no}/cars", async (string no, AppDbContext db, ITenantContext t) =>
{
    no = no.Trim().ToUpperInvariant();
    var r = await db.RetrieveRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TranspReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    var cars = await db.RetrieveReqCars.Where(c => c.OrgId == t.OrgId && c.ReqId == r.Id)
        .Select(c => new { c.Vin, c.StorageCode, c.DtlStatus }).ToListAsync();
    return Results.Ok(new { r.TranspReqNo, r.Status, count = cars.Count, cars });
}).RequireAuthorization();

app.MapPost("/api/retrievereqs/{no}/{action}", async (string no, string action, AppDbContext db, ITenantContext t) =>
{
    if (action is not ("approve" or "reject")) return Results.BadRequest(new { error = "action = approve|reject" });
    no = no.Trim().ToUpperInvariant();
    var r = await db.RetrieveRequests.FirstOrDefaultAsync(x => x.OrgId == t.OrgId && x.TranspReqNo == no);
    if (r is null) return Results.NotFound(new { no });
    if (r.Status != "Pending") return Results.BadRequest(new { error = "Chỉ duyệt/từ chối yêu cầu Đang xử lý." });
    r.Status = action == "approve" ? "Approved" : "Rejected"; r.DecidedAt = DateTime.Now;
    var dtl = r.Status;
    foreach (var c in await db.RetrieveReqCars.Where(c => c.OrgId == t.OrgId && c.ReqId == r.Id).ToListAsync())
        c.DtlStatus = dtl;
    await db.SaveChangesAsync();
    return Results.Ok(new { r.TranspReqNo, status = r.Status });
}).RequireAuthorization();

// ===== Phí bảo hiểm (Mst_InsuranceFee — port 1:1 FrmMst_InsuranceFee) =====
app.MapGet("/api/insfees", async (AppDbContext db, ITenantContext t, string? q) =>
{
    var query = db.InsuranceFees.Where(x => x.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Code.Contains(q) || (x.ContractNo ?? "").Contains(q));
    var items = await query.OrderBy(x => x.Code).Select(x => new
    { x.Code, x.InsCompanyCode, x.InsTypeCode, x.ContractNo, x.Fee, x.Percent, x.Status }).ToListAsync();
    return Results.Ok(new { count = items.Count, items });
}).RequireAuthorization();

app.MapPost("/api/insfees", async (InsFeeDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.Code)) return Results.BadRequest(new { error = "Cần Code." });
    var code = dto.Code.Trim().ToUpperInvariant();
    var x = await db.InsuranceFees.FirstOrDefaultAsync(y => y.OrgId == t.OrgId && y.Code == code);
    if (x is null) { x = new InsuranceFee { OrgId = t.OrgId, Code = code }; db.InsuranceFees.Add(x); }
    x.InsCompanyCode = dto.InsCompanyCode; x.InsTypeCode = dto.InsTypeCode; x.ContractNo = dto.ContractNo;
    x.Fee = dto.Fee; x.Percent = dto.Percent; x.Status = dto.Status ?? "1";
    await db.SaveChangesAsync();
    return Results.Ok(new { x.Code, x.Fee, x.Percent });
}).RequireAuthorization();

app.MapDelete("/api/insfees/{code}", async (string code, AppDbContext db, ITenantContext t) =>
{
    code = code.Trim().ToUpperInvariant();
    var x = await db.InsuranceFees.FirstOrDefaultAsync(y => y.OrgId == t.OrgId && y.Code == code);
    if (x is null) return Results.NotFound(new { code });
    db.InsuranceFees.Remove(x); await db.SaveChangesAsync();
    return Results.Ok(new { deleted = code });
}).RequireAuthorization();

// ===== Hạn mức phân bổ xe (Mst_Quota — port 1:1 FrmMngQuota) =====
app.MapGet("/api/quotas", async (AppDbContext db, ITenantContext t, string? period, string? dealer) =>
{
    var q = db.Quotas.Where(x => x.OrgId == t.OrgId);
    if (!string.IsNullOrWhiteSpace(period)) q = q.Where(x => x.Period == period);
    if (!string.IsNullOrWhiteSpace(dealer)) q = q.Where(x => x.DealerCode == dealer);
    var rows = await q.OrderBy(x => x.Period).ThenBy(x => x.DealerCode).Take(500).Select(x => new
    { x.DealerCode, x.ModelCode, x.Period, x.Qty, x.UsedQty, remain = x.Qty - x.UsedQty }).ToListAsync();
    return Results.Ok(new { count = rows.Count, totalQty = rows.Sum(r => r.Qty), totalUsed = rows.Sum(r => r.UsedQty), rows });
}).RequireAuthorization();

app.MapPost("/api/quotas", async (QuotaDto dto, AppDbContext db, ITenantContext t) =>
{
    if (string.IsNullOrWhiteSpace(dto.DealerCode) || string.IsNullOrWhiteSpace(dto.ModelCode) || string.IsNullOrWhiteSpace(dto.Period))
        return Results.BadRequest(new { error = "Cần DealerCode, ModelCode, Period." });
    var dealer = dto.DealerCode.Trim().ToUpperInvariant(); var model = dto.ModelCode.Trim().ToUpperInvariant(); var period = dto.Period.Trim();
    var x = await db.Quotas.FirstOrDefaultAsync(y => y.OrgId == t.OrgId && y.DealerCode == dealer && y.ModelCode == model && y.Period == period);
    if (x is null) { x = new Quota { OrgId = t.OrgId, DealerCode = dealer, ModelCode = model, Period = period }; db.Quotas.Add(x); }
    x.Qty = dto.Qty; if (dto.UsedQty.HasValue) x.UsedQty = dto.UsedQty.Value; x.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(new { x.DealerCode, x.ModelCode, x.Period, x.Qty, remain = x.Qty - x.UsedQty });
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
record BomDto(string BomCode, string ModelCode, string? MaintLevel, string? Status);
record BomLineDto(string PartSku, string? PartName, decimal Qty);
record WExtDto(string Vin, string? ItemCode, int ExtraMonths, decimal Fee);
record InsFeeDto(string Code, string? InsCompanyCode, string? InsTypeCode, string? ContractNo, decimal Fee, decimal Percent, string? Status);
record QuotaDto(string DealerCode, string ModelCode, string Period, int Qty, int? UsedQty);
record MortgageDto(string BankCode, List<string>? Vins);
record PmLineDto(string RefNo, decimal AmountAccum, decimal AmountCurrent);
record PmDto(string DealerCode, string? BankAccountSend, string? BankAccountReceive, List<PmLineDto>? Lines);
record GrtDto(string BankCode, string? BankGrtNo, string? GrtType, decimal GrtValue, DateTime? GrtDate, DateTime? DateExpired);
record GrtExpiryDto(DateTime? DateExpired);
record InvoiceLineDto(string? CarId, string? DealerCode, string InvoiceNo, string Vin, DateTime? InvoiceDate);
record InvoiceListDto(List<InvoiceLineDto>? Lines);
record BankBillCarDto(string Vin, string? EngineNo, string? LCNo, string? GuaranteeBankCode, decimal ClaimAmount);
record BankBillDto(string BankCode, DateTime? BankBillDate, List<BankBillCarDto>? Cars);
record BankBillReceiveDto(DateTime? BankBillReciveDate);
record TransReqCarDto(string Vin, string? DoNo, string? ColorCode, string? StorageCode);
record TransReqDto(string DealerCode, string TransporterCode, string? TransContractNo, List<TransReqCarDto>? Cars);
record TranspFeeDto(string ProvinceCodeFrom, string ProvinceCodeTo, string? DistrictCodeFrom, string? DistrictCodeTo, string TransporterCode, string ModelCode, decimal ValFee, int ExpectedDays);
record TransMinCarDto(string Vin, string? DoNo, string? ColorCode, string? EngineNo);
record TransMinDto(string DealerCode, string TransporterCode, List<TransMinCarDto>? Cars);
record HolidayDto(DateTime? Date, bool IsHoliday, string? Description);
record HolidayResetDto(int? Year, List<int>? WeekendDays);
record TransPlanDto(string VINPlan, string? Vin, string ModelCode, string DealerCode, string? StorageCode, string? FProvinceCode, string? TProvinceCode, string? TransporterCode, DateTime? ExpectedDate);
record RetrieveReqCarDto(string Vin, string? StorageCode);
record RetrieveReqDto(string DealerCode, string TransporterCode, string? Reason, List<RetrieveReqCarDto>? Cars);
record VinPairDto(string FVIN, string RVIN);
record MapVinDto(List<VinPairDto>? Pairs);
record VinPackingRowDto(string Vin, string? LoaiThung, string? ActualSpec, string? SerialNo, DateTime? InspectionDate);
record VinPackingDto(List<VinPackingRowDto>? Items);
record GpsClaimDto(string GpsDvNo, string? BeforeFixRemark, string? Remark);
record GpsInDevDto(string GpsDvNo, string? GpsBoxNo, string? Remark);
record GpsInDto(string? GpsInType, string StorageCode, string? Remark, List<GpsInDevDto>? Devices);
record GpsOutDto(string StorageCode, string? UserCodeReceived, string? Remark, List<GpsInDevDto>? Devices);
record PointRegisDto(string PointRegisCode, string DealerCode, string? PointRegisName, double MapLatitude, double MapLongitude, double Radius);
record GpsMapDto(string GpsDvNo, string Vin, string? DealerCode, string? DealerName, string? Address, string? StorageCode);
record SmViolateDto(string SalesManCode, string? SalesManName, string? DealerCode, string ViolateTypeId, DateTime? ViolateDateStart, DateTime? ViolateDateEnd, string? IdentityCardNo, string? PhoneNo, string? Remark);
record DlSalesManDto(string SMCode, string SMName, string? DealerCode, string? SMStatus, string? Sex, DateTime? DateOfBirth, string? PhoneNo, string? IdentityCardNo);
record DlGrantDto(string SMHyundaiCode);
record DlStatusDto(string SMStatus);
record CarMtnDto(string Vin, string? StorageCode, string? ModelCode, string? MtnType, DateTime? MtnDate, int? CycleDays, string? UserCode, string? Remark);
record MaintExtDto(string Vin, string? ModelCode, string? StorageCode, string? MtnExtRemark);
record DiscountDto(DateTime? EffectiveDate, decimal DiscountPercent, decimal PenaltyPercent, decimal PenaltyPercentTCKT, decimal FnExpPercent, decimal PmtDsTCGPercent, string? Status);
record DevicePriceDto(string SpecCode, string? SpecDescription, string? DeviceTypeCode, string DeviceCode, string? DeviceName, decimal Price, decimal VAT, DateTime? EffectiveDate, string? Status);
record TcgPriceDto(string SpecCode, decimal UnitPrice, string? Status);
record QuotaAdjustDto(string DealerCode, string ModelCode, string Period, int DeltaQty);
record RoServiceDto(string SerCode, string? SerName, string? Cause, string? Engineer, decimal Amount);
record RoPartDto(string PartCode, string? PartName, string? Unit, decimal NeedQty, decimal UnitPrice, string? Note);
record RepairOrderDto(string LicensePlate, string? Vin, string? CusName, string? Km, DateTime? CheckInDate, DateTime? PlanedDeliveryDate, string? CusRequest, string? CarStatus, bool CusWaiting, List<RoServiceDto>? Services, List<RoPartDto>? Parts);
record RoAdvanceDto(string ToStatus);
record StockReqLineDto(string PartCode, string? PartName, string? Location, decimal Quantity, string? Unit);
record StockReqDto(string RONo, bool FromRO, List<StockReqLineDto>? Lines);
record ReceptionDto(string PlateNo, string? ModelName, string? CusName, string? CusAddress, string? CusPhoneNo, string? CusRequest);
record ReceptionLinkDto(string RONO);
record StockInLineDto(string PartCode, string? PartName, string? Location, decimal Quantity, decimal Price, decimal VAT);
record StockInDto(DateTime? StockInDate, string? StockInType, string WarehouseCode, string? Staff, List<StockInLineDto>? Lines);
record StockOutLineDto(string PartCode, string? PartName, string? Location, decimal Quantity);
record StockOutDto(DateTime? StockOutDate, string? StockOutType, string WarehouseCode, string? Reason, List<StockOutLineDto>? Lines);
record PartPriceDto(string PartCode, string? PartName, decimal Price, decimal VAT, DateTime? EffectiveDate, string? Status);
record CustomerCarDto(string? Vin, string? PlateNo, string? FrameNo, string? EngineNo, string? ModelCode, string? ColorCode, string? PlateColorCode, string? CusCode, string? CusName, string? CusPhone, DateTime? SaleDate);
record CustomerCareDto(string? CareType, string? RONo, string? PlateNo, string? CusName, string? CusPhone, DateTime? ContactDate);
record CareContactDto(string? Result);
record ServiceCustomerDto(string? CusCode, string CusName, string? CusTypeID, string? Address, string? Mobile, string? Tel, string? Email, string? TaxCode, string? Sex, DateTime? DOB, string? ContName, string? ContMobile, string? ContTel, string? ContEmail);
record OrderPartLineDto(string PartCode, string? PartName, decimal OrderQty, decimal Price);
record OrderPartDto(string SupplierCode, string? WarehouseCode, List<OrderPartLineDto>? Lines);
record OrderComplainDto(string OrderPartNo, string? ComplainType, string? Content);
record OrderComplainActDto(string? Resolution);
record SupplierPaymentDto(string SupplierCode, string? OrderPartNo, decimal Amount, DateTime? PaymentDate);
record ReqPartPriceLineDto(string PartCode, string? PartName, decimal ReqQty);
record ReqPartPriceDto(List<ReqPartPriceLineDto>? Lines);
record ReqQuoteItemDto(string? PartCode, decimal QuotedPrice);
record ReqQuoteDto(List<ReqQuoteItemDto>? Quotes);
record GroupRepairDto(string GroupRCode, string GroupRName, string? Note, string? Status);
record EngineerDto(string EngineerNo, string EngineerName, string? GroupRCode, string? Note, string? Status);
record CampaignContactDto(string? PlateNo, string? CusName, string? Address);
record CampaignDto(string CamNo, string CamName, DateTime? StartDate, DateTime? FinishDate, string? Content, List<CampaignContactDto>? Contacts);
record ServiceInvoiceDto(string RONo, decimal VatPercent, decimal DiscountAmount, string? PaymentType);
record POCommandLineDto(string SpecCode, string? SpecDesc, string? ColorCode, string? PortCode, string? PlantCode, int Quantity);
record POCommandDto(string OrderMonth, List<POCommandLineDto>? Lines);
record RegisterOrgDto(string Name);
