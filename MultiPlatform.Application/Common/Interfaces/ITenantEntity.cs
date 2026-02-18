using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiPlatform.Application.Common.Interfaces
{
    public interface ITenantEntity
    {
        Guid TenantId { get; set; }
    }
}