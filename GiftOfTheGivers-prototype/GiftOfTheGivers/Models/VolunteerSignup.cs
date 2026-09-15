using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public class VolunteerSignup
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [Required, StringLength(300)]
    [Display(Name = "Skills")]
    public string Skills { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [Display(Name = "Availability")]
    public string Availability { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
}
