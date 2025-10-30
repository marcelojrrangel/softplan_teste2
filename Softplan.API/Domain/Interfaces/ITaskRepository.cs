using TaskEntity = Softplan.API.Domain.Entities.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Softplan.API.Domain.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskEntity>
    {
        Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string userId);
    }
}
