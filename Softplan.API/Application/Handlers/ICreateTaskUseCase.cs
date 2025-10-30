using Softplan.API.Presentation.DTOs;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public interface ICreateTaskUseCase
    {
        Task<TaskResponse> ExecuteAsync(CreateTaskRequest request);
    }
}
