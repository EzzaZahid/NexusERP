using NexusERP_D2.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusERP_D2.Data;
using NexusERP_D2.Models;

namespace NexusERP_D2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            var connectionString =
                builder.Configuration.GetConnectionString(
                    "DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(connectionString));
            //
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            // MVC
            builder.Services.AddControllersWithViews();

            // Identity
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan =
                        TimeSpan.FromMinutes(5);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Create application roles
            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider
            //        .GetRequiredService<RoleManager<IdentityRole>>();

            //    string[] roles = { "Admin", "Employee" };

            //    foreach (var role in roles)
            //    {
            //        if (!await roleManager.RoleExistsAsync(role))
            //        {
            //            await roleManager.CreateAsync(
            //                new IdentityRole(role));
            //        }
            //    }
            //}

            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedAsync(
                    scope.ServiceProvider,
                    app.Configuration);
            }


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            await app.RunAsync();
        }
    }
}
