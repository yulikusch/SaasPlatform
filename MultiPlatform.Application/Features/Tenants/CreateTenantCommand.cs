using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Core.Entities;

namespace MultiPlatform.Application.Features.Tenants
{
    public class CreateTenantCommand
    {
        public string Name { get; set; } = default!;
        public string Subdomain { get; set; } = default!;
    }
}