using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Models;

// Extends the built-in Identity user with a display name.
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
