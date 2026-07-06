using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Identity;
using TaskFlow.Interfaces;
using TaskFlow.Mapping;
using TaskFlow.Models;
using TaskFlow.SeedData;
using TaskFlow.Services;
using TaskFlow.Services.Notifications;
using TaskFlow.ViewModels.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaimsPrincipalFactory>();

builder.Services.Configure<AttachmentSettings>(builder.Configuration.GetSection(CustomSettings.AttachmentSettings));

//policy improvised version of the roles we can centralize the authorize option and we can apply flexible logics in the policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.SuperAdminOnly,
        policy => policy.RequireRole(Roles.SuperAdmin));

    options.AddPolicy(Policies.CompanyAccess,
        policy => policy.RequireRole(Roles.SuperAdmin, Roles.CompanyAdmin));

    options.AddPolicy(Policies.EmployeeAccess,
        policy => policy.RequireRole(Roles.Employee, Roles.CompanyAdmin));
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserServices, CurrentUserServices>();
builder.Services.AddScoped<ITaskActivityService,TaskActivityService>();
builder.Services.AddScoped<ITaskAttachmentService, TaskAttachmentService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<EmployeeService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();
//Runs once on the http request and clears out all the instance created to run it
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();

    // Apply all pending migrations first
    await context.Database.MigrateAsync();

    // Seed Identity (Roles, Admin User, etc.)
    await IdentitySeeder.SeedRolesAndAdminAsync(services);

    // Seed application lookup data
    await InitialSeeder.InitializeAsync(context);
}

//Please dont change the order let it stay as it is, otherwise it will break the code and the application will not run.
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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
