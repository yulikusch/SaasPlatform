using Microsoft.EntityFrameworkCore;
using MultiPlatform.Infrastructure.Data;
using MultiPlatform.Application.Common.Interfaces;
using MultiPlatform.Infrastructure.Tenancy;
using MultiPlatform.Infrastructure.Seed;
using MultiPlatform.Application.Features.Tenants;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(sp =>
    sp.GetRequiredService<ApplicationDbContext>());


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<CreateTenantHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


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

app.UseSwagger();
app.UseSwaggerUI();

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
