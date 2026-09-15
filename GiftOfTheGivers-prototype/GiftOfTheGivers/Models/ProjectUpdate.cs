using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public class ProjectUpdate
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    [StringLength(100)]
    public string PostedBy { get; set; } = string.Empty;

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
}
