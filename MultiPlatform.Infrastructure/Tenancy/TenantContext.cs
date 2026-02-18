using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Core.Entities;

namespace MultiPlatform.Infrastructure.Tenancy
{
    public class TenantContext : ITenantContext
    {
        public Tenant? CurrentTenant { get; set; }
    }
}