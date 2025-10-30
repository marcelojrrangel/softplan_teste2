using Microsoft.Extensions.Logging;
using Softplan.API.Presentation.DTOs;
using Softplan.API.Domain.Interfaces;
using TaskEntity = Softplan.API.Domain.Entities.Task;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class GetTasksByUserUseCase : IGetTasksByUserUseCase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<GetTasksByUserUseCase> _logger;

        public GetTasksByUserUseCase(ITaskRepository taskRepository, ILogger<GetTasksByUserUseCase> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskResponse>> ExecuteAsync(string userId)
        {
            _logger.LogInformation("Retrieving tasks for user {UserId}", userId);

            var tasks = await _taskRepository.GetByUserIdAsync(userId);

            _logger.LogInformation("Retrieved {TaskCount} tasks for user {UserId}", tasks.Count(), userId);
            return tasks.Select(TaskMapper.ToResponse);
        }
    }
}
