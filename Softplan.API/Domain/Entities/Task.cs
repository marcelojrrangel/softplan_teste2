using System.ComponentModel.DataAnnotations;

namespace Softplan.API.Domain.Entities;

public class Task
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}
