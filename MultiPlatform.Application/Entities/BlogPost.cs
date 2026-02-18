using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPlatform.Application.Common.Interfaces;

namespace MultiPlatform.Application.Entities
{
    public class BlogPost : ITenantEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid TenantId { get; set; }
    }
}