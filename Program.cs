using eMobileShop.Data;
using eMobileShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Services
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //Authentication and authorization
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
    builder.Services.AddMemoryCache();
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    builder.Services.AddSession(options =>
    {
        // Set a short timeout for easy testing.
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        //options.Cookie.HttpOnly = true;
        // Make the session cookie essential
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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
    app.UseSession();

    //Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapAreaControllerRoute(
                name: "AdminArea",
                areaName: "Admin",
                pattern: "Admin/{controller=Category}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    //Seed database
    AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

    app.Run();
}
catch (Exception ex)
{
    throw;
}