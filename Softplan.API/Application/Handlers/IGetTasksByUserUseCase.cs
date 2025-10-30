using Softplan.API.Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers
{
    public interface IGetTasksByUserUseCase
    {
        Task<IEnumerable<TaskResponse>> ExecuteAsync(string userId);
    }
}
