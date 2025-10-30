namespace Softplan.API.Presentation.DTOs
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}