using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;

namespace MultiPlatform.Application.Features.Tenants
{
    public class ActivateTenantHandler
    {
        private readonly IApplicationDbContext _db;

        public ActivateTenantHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(ActivateTenantCommand command)
        {
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(x => x.Id == command.TenantId);

            if (tenant == null)
                throw new Exception("Tenant tidak ditemukan");

            tenant.IsActive = true;

            await _db.SaveChangesAsync(CancellationToken.None);
        }
    }
}