using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Http;


namespace MultiPlatform.Infrastructure.Tenancy;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
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
        if (!string.IsNullOrEmpty(subdomain) && subdomain != "localhost")
        {
            var tenant = await db.Tenants
                .FirstOrDefaultAsync(x => x.Subdomain == subdomain && x.IsActive);

            tenantContext.CurrentTenant = tenant;
        }

        await _next(context);
    }
}
