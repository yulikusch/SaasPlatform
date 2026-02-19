using Microsoft.EntityFrameworkCore;
using MultiPlatform.Infrastructure.Data;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Infrastructure.Tenancy;
using MultiPlatform.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(sp =>
    sp.GetRequiredService<ApplicationDbContext>());


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITenantContext, TenantContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<TenantResolutionMiddleware>();
app.UseMiddleware<TenantValidationMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await TenantSeeder.SeedAsync(db);
}


app.Run();
