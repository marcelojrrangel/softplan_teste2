using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers;

public interface IDeleteTaskUseCase
{
    Task<bool> ExecuteAsync(int id);
}
