using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data;

// Extends IdentityDbContext so ASP.NET Core Identity (users, roles, logins) share
// the same database as the application's own tables.
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<VolunteerSignup> VolunteerSignups => Set<VolunteerSignup>();
    public DbSet<ProjectUpdate> ProjectUpdates => Set<ProjectUpdate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Donation>()
            .Property(d => d.Amount)
            .HasColumnType("decimal(18,2)");
    }
}
