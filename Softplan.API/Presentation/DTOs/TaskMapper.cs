using TaskEntity = Softplan.API.Domain.Entities.Task;

namespace Softplan.API.Presentation.DTOs
{
    public static class TaskMapper
    {
    public static TaskEntity ToModel(string title, string? description, DateTime? dueDate, string userId)
    {
        return new TaskEntity
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            UserId = userId
        };
    }

        public static TaskResponse ToResponse(TaskEntity task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedDate = task.CreatedDate,
                DueDate = task.DueDate,
                CompletedDate = task.CompletedDate,
                UserId = task.UserId,
                IsCompleted = task.IsCompleted
            };
        }
    }
}