using Comasy.Core;
using Comasy.Data;
using Microsoft.AspNetCore.Identity;

namespace Comasy.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataLayer(builder.Configuration);

            builder.Services.AddServiceLayer();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ComasyDbContext>() 
            .AddDefaultTokenProviders(); 

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Denied";
            }); 

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ComasyDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var adminPassword = builder.Configuration["SeedAdminPassword"] 
                                    ?? throw new InvalidOperationException(
                                        "SeedAdminPassword saknas i konfig.");

                await DbSeeder.SeedAsync(context, userManager, roleManager, adminPassword);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            
            app.MapControllerRoute(
                name: "home",
                pattern: "",
                defaults: new { controller = "Page", action = "Index", slug = "valkommen" });
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.MapControllerRoute(
                name: "page",
                pattern: "{slug}",
                defaults: new  { controller = "Page", action = "Index" })
                .WithStaticAssets();

            app.Run();
        }
    }
}
