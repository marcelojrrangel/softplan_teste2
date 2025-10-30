using MediatR;
using Softplan.API.Presentation.DTOs;
using System.Collections.Generic;

namespace Softplan.API.Application.Queries
{
    public class GetTasksByUserQuery : IRequest<IEnumerable<TaskResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}