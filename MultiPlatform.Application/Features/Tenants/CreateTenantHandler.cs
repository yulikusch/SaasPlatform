using Microsoft.EntityFrameworkCore;
using MultiPlatform.Core.Entities;
using MultiPlatform.Application.Common.Interfaces;

namespace MultiPlatform.Application.Features.Tenants;

public class CreateTenantHandler
{
    private readonly IApplicationDbContext _db;

    public CreateTenantHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
    CreateTenantCommand command,
    CancellationToken cancellationToken)
    {
        var exists = await _db.Tenants
            .AnyAsync(x => x.Subdomain == command.Subdomain, cancellationToken);

        if (exists)
            throw new Exception("Subdomain sudah digunakan");

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Subdomain = command.Subdomain,
            IsActive = true
        };

        _db.Tenants.Add(tenant);
        await _db.SaveChangesAsync(cancellationToken);

        return tenant.Id;
    }

}
