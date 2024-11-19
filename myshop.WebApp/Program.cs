using Microsoft.EntityFrameworkCore;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using myshop.Utilities;
using Stripe;
using myshop.Entities.Models;
using myshop.DataAccess.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnE_Shop")));

// Configure Stripe settings
builder.Services.Configure<StripeDetails>(builder.Configuration.GetSection("Stripe"));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4);
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Add custom services
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Seed Roles and Users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();

    try
    {
        // Get required services
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        logger.LogInformation("Starting seeding roles...");
        await DefaultRoles.SeedAsync(roleManager, logger);
        logger.LogInformation("Roles seeded successfully.");

        logger.LogInformation("Starting seeding admin user...");
        await DefaultUsers.SeedAdminUser(userManager, logger);
        logger.LogInformation("Admin user seeded successfully.");

        logger.LogInformation("Starting seeding normal users...");
        await DefaultUsers.SeedUserAsync(userManager, logger);
        logger.LogInformation("Normal users seeded successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during seeding.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// Map Razor Pages
app.MapRazorPages();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Identity",
    pattern: "{area=Identity}/{controller=Home}/{action=Index}/{id?}");

app.Run();
