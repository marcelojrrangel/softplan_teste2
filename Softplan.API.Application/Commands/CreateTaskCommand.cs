using MediatR;
using Softplan.API.Application.DTOs;
using System;

namespace Softplan.API.Application.Commands
{
    public class CreateTaskCommand : IRequest<TaskResponse>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}