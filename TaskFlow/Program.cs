using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region comment
// ------------------------------------------------------------
// Dependency Injection (DI) Registration
// ------------------------------------------------------------
//
// ASP.NET Core uses Dependency Injection to manage object creation automatically.
//
// Instead of manually creating objects like:
//      var service = new CompanyService();
//
// We register services here so ASP.NET Core can:
// 1. Create the object when needed
// 2. Manage its lifetime (Scoped, Transient, Singleton)
// 3. Inject it automatically into controllers or other services
//
// Example:
//      builder.Services.AddScoped<CompanyService>();
//
// This means:
// - "Whenever CompanyService is required, create and provide one per HTTP request."
//
// ------------------------------------------------------------
// Why this is important:
// ------------------------------------------------------------
// - Keeps code clean and loosely coupled
// - Avoids manual object creation everywhere
// - Makes testing and maintenance easier
// - Central place to manage application dependencies
//
// ------------------------------------------------------------
// Real Flow:
// ------------------------------------------------------------
// Controller → asks for Service
// ASP.NET Core DI Container → provides Service
// Service → uses DbContext → accesses Database
//
// ------------------------------------------------------------
#endregion
builder.Services.AddScoped<CompanyService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
