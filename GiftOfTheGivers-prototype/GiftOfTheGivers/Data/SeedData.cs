using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data;

// Runs once at startup: creates the SQLite database if it doesn't exist yet,
// seeds the Employee/Donor roles, a demo Employee login, and a couple of
// sample relief-project updates so the Home page and dashboard aren't empty.
//
// NOTE: EnsureCreatedAsync() is used instead of migrations to keep the
// prototype simple to run (no "dotnet ef" step required). For Part 2 /
// production, switch to proper EF Core migrations.
public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var roleName in new[] { "Employee", "Donor" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        const string employeeEmail = "employee@gotg.org";
        if (await userManager.FindByEmailAsync(employeeEmail) is null)
        {
            var employee = new ApplicationUser
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                FullName = "Demo Employee",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(employee, "Employee123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(employee, "Employee");
            }
        }

        if (!await context.ProjectUpdates.AnyAsync())
        {
            context.ProjectUpdates.AddRange(
                new ProjectUpdate
                {
                    Title = "Flood Relief - KwaZulu-Natal",
                    Content = "Teams have distributed food parcels and clean water to over 500 " +
                              "families affected by this week's flooding.",
                    PostedBy = "Demo Employee",
                    PostedDate = DateTime.UtcNow.AddDays(-2)
                },
                new ProjectUpdate
                {
                    Title = "Medical Outreach - Eastern Cape",
                    Content = "Mobile clinics have been deployed to reach communities cut off by " +
                              "recent storms, providing basic medical care and supplies.",
                    PostedBy = "Demo Employee",
                    PostedDate = DateTime.UtcNow.AddDays(-5)
                });

            await context.SaveChangesAsync();
        }
    }
}
