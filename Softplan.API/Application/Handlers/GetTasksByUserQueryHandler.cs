using MediatR;
using Softplan.API.Application.Queries;
using Softplan.API.Domain.Interfaces;
using Softplan.API.Presentation.DTOs;
using TaskEntity = Softplan.API.Domain.Entities.Task;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class GetTasksByUserQueryHandler : IRequestHandler<GetTasksByUserQuery, IEnumerable<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksByUserQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetTasksByUserQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByUserIdAsync(request.UserId);
            return tasks.Select(TaskMapper.ToResponse);
        }
    }
}