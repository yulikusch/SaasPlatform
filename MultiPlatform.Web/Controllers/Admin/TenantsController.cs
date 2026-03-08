using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Application.Features.Tenants;

namespace MultiPlatform.Web.Controllers.Admin;

[ApiController]
[Route("admin/tenants")]
public class TenantsController : ControllerBase
{
    private readonly IApplicationDbContext _db;
    private readonly CreateTenantHandler _createTenant;

    public TenantsController(
        IApplicationDbContext db,
        CreateTenantHandler createTenant)
    {
        _db = db;
        _createTenant = createTenant;
    }

    // ===============================
    // CREATE TENANT
    // ===============================
    [HttpPost]
    public async Task<IActionResult> Create(CreateTenantCommand command)
    {
        var id = await _createTenant.Handle(
            command,
            HttpContext.RequestAborted
        );

        return Ok(new { TenantId = id });
    }



    // ===============================
    // LIST TENANTS (SUPER ADMIN ONLY)
    // ===============================
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var tenants = await _db.Tenants
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Subdomain,
                x.IsActive
            })
            .ToListAsync();

        return Ok(tenants);
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var handler = new ActivateTenantHandler(_db);
        await handler.Handle(new ActivateTenantCommand(id));

        return Ok("Tenant activated");
    }


    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var handler = new DeactivateTenantHandler(_db);
        await handler.Handle(new DeactivateTenantCommand(id));

        return Ok("Tenant deactivated");
    }


}
