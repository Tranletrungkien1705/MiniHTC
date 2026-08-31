using Microsoft.EntityFrameworkCore;
using MiniHTC.Models;
namespace MiniHTC.Data;
public static class Seeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        if (!await db.Orgs.AnyAsync(o => o.Id == TenantContext.DefaultOrgId))
            db.Orgs.Add(new Org { Id = TenantContext.DefaultOrgId, Name = "HTC", ApiKey = "demo-htc" });
        if (!await db.Areas.AnyAsync())
            db.Areas.AddRange(
                new Area { OrgId = TenantContext.DefaultOrgId, AreaCode = "MB", AreaName = "Miền Bắc", Status = "1" },
                new Area { OrgId = TenantContext.DefaultOrgId, AreaCode = "MT", AreaName = "Miền Trung", Status = "1" },
                new Area { OrgId = TenantContext.DefaultOrgId, AreaCode = "MN", AreaName = "Miền Nam", Status = "1" });
        await db.SaveChangesAsync();
    }
}
