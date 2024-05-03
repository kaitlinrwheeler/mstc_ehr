using EHRApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using EHRApplication.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //This sets the default connection for services in this file.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        //This will initialize the logger class for all of the controllers.
        builder.Services.AddSingleton<ILogService>(loger =>
        {
            return new LogService(connectionString);
        });

        //More Microsoft identity stuff
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Set the timeout to 30 minutes
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true; // Extend expiration time on activity
        });
        //Microsoft identity stuff
        builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();



        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //This will initialize the service class for all of the controllers.
        builder.Services.AddSingleton<IListService>(provider =>
        {
            return new ListService(connectionString);
        });

        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 4 * 1024 * 1024; // 4 MB
        });

        builder.Services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = 4 * 1024 * 1024; // 4 MB
        });



        //Tries to build the application
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

        //This will route the user to whatever page you tell it to down below. it is set to the landing page right now.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LandingPage}/{id?}"); // changed from action=Index to action=LandingPage
        
    app.Run();
    }
}