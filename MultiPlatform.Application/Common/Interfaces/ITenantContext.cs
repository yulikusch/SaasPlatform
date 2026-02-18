using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Core.Entities;

namespace MultiPlatform.Application.Common.Interfaces
{
    public interface ITenantContext
    {
        Tenant? CurrentTenant { get; set; }
    }
}