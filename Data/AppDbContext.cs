using Microsoft.EntityFrameworkCore;
using MiniHTC.Models;
namespace MiniHTC.Data;
public sealed class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<Org> Orgs => Set<Org>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<MasterItem> Masters => Set<MasterItem>();
    public DbSet<Dealer> Dealers => Set<Dealer>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Org>().HasIndex(x => x.ApiKey).IsUnique();
        b.Entity<Area>().HasIndex(x => new { x.OrgId, x.AreaCode }).IsUnique();
        b.Entity<MasterItem>().HasIndex(x => new { x.OrgId, x.Category, x.Code }).IsUnique();
        b.Entity<Dealer>().HasIndex(x => new { x.OrgId, x.DealerCode }).IsUnique();
    }
}
