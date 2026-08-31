using Microsoft.EntityFrameworkCore;
using MiniHTC.Models;
namespace MiniHTC.Data;
public sealed class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<Org> Orgs => Set<Org>();
    public DbSet<Area> Areas => Set<Area>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Org>().HasIndex(x => x.ApiKey).IsUnique();
        b.Entity<Area>().HasIndex(x => new { x.OrgId, x.AreaCode }).IsUnique();
    }
}
