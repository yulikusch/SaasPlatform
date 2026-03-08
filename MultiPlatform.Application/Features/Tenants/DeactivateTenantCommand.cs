using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiPlatform.Application.Features.Tenants
{
    public record DeactivateTenantCommand(Guid TenantId);
}