using System.Threading.Tasks;

namespace Softplan.API.Application.Handlers;

public interface IPutTaskUseCase
{
    Task<bool> ExecuteAsync(int id);
}
