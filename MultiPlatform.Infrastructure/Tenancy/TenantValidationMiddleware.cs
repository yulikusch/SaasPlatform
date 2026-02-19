using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Http;


namespace MultiPlatform.Infrastructure.Tenancy
{
    public class TenantValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
        {
            var tenant = tenantContext.CurrentTenant;

            if (tenant == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Tenant tidak ditemukan.");
                return;
            }

            if (!tenant.IsActive)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Tenant tidak aktif.");
                return;
            }

            await _next(context);
        }
    }
}