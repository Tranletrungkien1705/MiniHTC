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
record RegisterOrgDto(string Name);
