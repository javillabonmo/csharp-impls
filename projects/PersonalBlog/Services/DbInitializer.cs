using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Models.Auth;
using PersonalBlog.Persistence;

namespace PersonalBlog.Services;

public static class DbInitializer
{
    /// <summary>
    ///     Seed de la base de datos: crea roles y un usuario admin por defecto.
    /// </summary>
    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        // Aplicar migraciones pendientes 
        await context.Database.MigrateAsync();

        // ───────────── 1. Crear roles ─────────────
        var roles = new[] { UserTypeOptions.Admin.ToString(), UserTypeOptions.User.ToString() };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        // ───────────── 2. Crear usuario admin por defecto ─────────────
        const string adminEmail = "admin@personalblog.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserTypeOptions.Admin.ToString());
            }
        }
    }
}
