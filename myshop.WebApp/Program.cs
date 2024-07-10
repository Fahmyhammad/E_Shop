using Microsoft.EntityFrameworkCore;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using myshop.Utilities;
using Stripe;
using myshop.DataAccess.DbInitializer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnE_Shop")));


builder.Services.Configure<StripeDetails>(builder.Configuration.GetSection("Stripe"));


builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4))
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddSingleton<IEmailSender,EmailSender>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

SeedDb();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();


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

void SeedDb()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initializer();
    }
}
