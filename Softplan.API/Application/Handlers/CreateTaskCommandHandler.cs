using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Softplan.API.Application.Commands;
using Softplan.API.Presentation.DTOs;
using Softplan.API.Domain.Interfaces;
using TaskEntity = Softplan.API.Domain.Entities.Task;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TaskEntity> _validator;
        private readonly ILogger<CreateTaskCommandHandler> _logger;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, IValidator<TaskEntity> validator, ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating task for user {UserId}", request.UserId);

            var task = TaskMapper.ToModel(request.Title, request.Description, request.DueDate, request.UserId);

            var validationResult = await _validator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Task validation failed: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            await _taskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Task created successfully with ID {TaskId}", task.Id);
            return TaskMapper.ToResponse(task);
        }
    }
}
