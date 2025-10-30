using MediatR;
using Softplan.API.Application.Commands;
using Softplan.API.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            var resultTask = await _taskRepository.GetByIdAsync(request.Id);

            if (resultTask == null)
            {
                return false;
            }

            resultTask.IsCompleted = true;
            resultTask.CompletedDate = DateTime.UtcNow;

            _taskRepository.Update(resultTask);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}