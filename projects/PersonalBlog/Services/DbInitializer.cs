// <copyright file="DbInitializer.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Models.Auth;
using PersonalBlog.Persistence;

namespace PersonalBlog.Services;

/// <summary>
/// Seeds the database with initial data including roles and an admin user.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Seeds the database: creates roles and a default admin user.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserTypeOptions.Admin.ToString());
            }
        }
    }
}
