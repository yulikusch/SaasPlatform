using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MultiPlatform.Core.Entities;


namespace MultiPlatform.Infrastructure.Tenancy;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public TenantResolutionMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(
    HttpContext context,
    ApplicationDbContext db,
    ITenantContext tenantContext)
    {
        var host = context.Request.Host.Host;
        Console.WriteLine("HOST = " + host);

        var subdomain = host.Split('.').FirstOrDefault();
        Console.WriteLine("SUBDOMAIN = " + subdomain);

        Tenant? tenant = null;

        // =========================
        // 1. Try resolve from subdomain
        // =========================
        if (!string.IsNullOrEmpty(subdomain) && subdomain != "localhost")
        {
            tenant = await db.Tenants
                .FirstOrDefaultAsync(x => x.Subdomain == subdomain && x.IsActive);
        }

        // =========================
        // 2. Fallback to default tenant
        // =========================
        if (tenant == null)
        {
            var defaultSubdomain = _configuration["MultiTenant:DefaultTenant"];

            Console.WriteLine("DEFAULT TENANT = " + defaultSubdomain);

            tenant = await db.Tenants
                .FirstOrDefaultAsync(x => x.Subdomain == defaultSubdomain && x.IsActive);
        }

        // =========================
        // 3. If still null → reject
        // =========================
        if (tenant == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Tenant tidak ditemukan.");
            return;
        }

        tenantContext.SetTenant(tenant);

        await _next(context);
    }

}
