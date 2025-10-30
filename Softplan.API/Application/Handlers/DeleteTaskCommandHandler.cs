using MediatR;
using Softplan.API.Application.Commands;
using Softplan.API.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var resultTask = await _taskRepository.GetByIdAsync(request.Id);

            if (resultTask == null)
            {
                return false;
            }

            _taskRepository.Delete(resultTask);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}