using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Infrastructure.Data;
using MultiPlatform.Core.Entities;

namespace MultiPlatform.Infrastructure.Seed
{
    public class TenantSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            if (!db.Tenants.Any())
            {
                db.Tenants.Add(new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = "Demo Store",
                    Subdomain = "demo",
                    IsActive = true
                });

                await db.SaveChangesAsync();
            }
        }
    }
}