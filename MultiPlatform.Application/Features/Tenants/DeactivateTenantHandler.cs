using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;

namespace MultiPlatform.Application.Features.Tenants
{
    public class DeactivateTenantHandler
    {
        private readonly IApplicationDbContext _db;

        public DeactivateTenantHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeactivateTenantCommand command)
        {
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(x => x.Id == command.TenantId);

            if (tenant == null)
                throw new Exception("Tenant tidak ditemukan");

            tenant.IsActive = false;

            await _db.SaveChangesAsync(CancellationToken.None);
        }
    }
}