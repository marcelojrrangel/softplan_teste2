using Microsoft.Extensions.Logging;
using Softplan.API.Domain.Interfaces;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers;

public class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTaskUseCase> _logger;

    public DeleteTaskUseCase(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ILogger<DeleteTaskUseCase> logger)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        _logger.LogInformation("Deleting task with ID {TaskId}", id);

        var resultTask = await _taskRepository.GetByIdAsync(id);

        if (resultTask == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", id);
            return false;
        }

        _taskRepository.Delete(resultTask);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task with ID {TaskId} deleted successfully", id);
        return true;
    }
}
