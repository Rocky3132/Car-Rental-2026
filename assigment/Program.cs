using assigment.Data;// 1. This fixes the "Services" error
using assigment.Servicess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog; // 2. This is for Task 3 (Logging)

namespace assigment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- TASK 3: LOGGING SETUP ---
            // This sets up Serilog to write to a file named 'applog.txt'
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Default Connection String
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            // --- TASK 1 & 3: REGISTER SERVICE ---
            // 3. This registers your UserService so it can be used in Controllers and Tests
            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // --- TASK 3: ERROR HANDLING ---
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                // This ensures errors are handled gracefully (User Story 3)
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}