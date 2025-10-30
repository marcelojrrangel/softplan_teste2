using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Softplan.API.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class PutTaskUseCase : IPutTaskUseCase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PutTaskUseCase> _logger;

        public PutTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ILogger<PutTaskUseCase> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            _logger.LogInformation("Completing task with ID {TaskId}", id);

            var resultTask = await _taskRepository.GetByIdAsync(id);

            if (resultTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return false;
            }

            resultTask.IsCompleted = true;
            resultTask.CompletedDate = DateTime.UtcNow;

            _taskRepository.Update(resultTask);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Task with ID {TaskId} completed successfully", id);
            return true;
        }
    }
}
