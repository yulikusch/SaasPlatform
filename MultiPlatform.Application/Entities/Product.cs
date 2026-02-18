using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Application.Common.Interfaces;

namespace MultiPlatform.Application.Entities
{
    public class Product : ITenantEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public Guid TenantId { get; set; }
    }
}