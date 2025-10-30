using System.ComponentModel.DataAnnotations;

namespace Softplan.API.Application.DTOs
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}