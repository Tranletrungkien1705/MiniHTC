using Microsoft.EntityFrameworkCore;
using MiniHTC.Models;
namespace MiniHTC.Data;
public sealed class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<Org> Orgs => Set<Org>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<MasterItem> Masters => Set<MasterItem>();
    public DbSet<Dealer> Dealers => Set<Dealer>();
    public DbSet<CarPrice> CarPrices => Set<CarPrice>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<SalesMan> SalesMen => Set<SalesMan>();
    public DbSet<PdiRequest> PdiRequests => Set<PdiRequest>();
    public DbSet<CarRetrieve> CarRetrieves => Set<CarRetrieve>();
    public DbSet<CarCancel> CarCancels => Set<CarCancel>();
    public DbSet<SysConfig> Configs => Set<SysConfig>();
    public DbSet<BusinessPlan> BusinessPlans => Set<BusinessPlan>();
    public DbSet<TestDrive> TestDrives => Set<TestDrive>();
    public DbSet<WarrantyClaimTC> WarrantyClaims => Set<WarrantyClaimTC>();
    public DbSet<SupplierPO> SupplierPOs => Set<SupplierPO>();
    public DbSet<Bom> Boms => Set<Bom>();
    public DbSet<BomLine> BomLines => Set<BomLine>();
    public DbSet<WarrantyExtension> WarrantyExts => Set<WarrantyExtension>();
    public DbSet<InsuranceFee> InsuranceFees => Set<InsuranceFee>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Org>().HasIndex(x => x.ApiKey).IsUnique();
        b.Entity<Area>().HasIndex(x => new { x.OrgId, x.AreaCode }).IsUnique();
        b.Entity<MasterItem>().HasIndex(x => new { x.OrgId, x.Category, x.Code }).IsUnique();
        b.Entity<Dealer>().HasIndex(x => new { x.OrgId, x.DealerCode }).IsUnique();
    }
}
