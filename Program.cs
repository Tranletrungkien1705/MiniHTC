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
record RegisterOrgDto(string Name);
