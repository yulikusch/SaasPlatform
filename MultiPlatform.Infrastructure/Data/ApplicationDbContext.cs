using Microsoft.EntityFrameworkCore;
using MultiPlatform.Core.Entities;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Application.Entities;



namespace MultiPlatform.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Product> Products => Set<Product>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var tenantId = _tenantContext.CurrentTenant?.Id;

        modelBuilder.Entity<BlogPost>()
            .HasQueryFilter(x => x.TenantId == tenantId);

        modelBuilder.Entity<Product>()
            .HasQueryFilter(x => x.TenantId == tenantId);
    }

    public override Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.CurrentTenant?.Id;

        if (tenantId != null)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.State == EntityState.Added &&
                    e.Entity is ITenantEntity);

            foreach (var entry in entries)
            {
                ((ITenantEntity)entry.Entity).TenantId = tenantId.Value;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


}
