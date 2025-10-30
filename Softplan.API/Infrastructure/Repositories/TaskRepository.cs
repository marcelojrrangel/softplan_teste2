using Microsoft.EntityFrameworkCore;
using Softplan.API.Infrastructure.Data;
using Softplan.API.Domain.Interfaces;
using TaskEntity = Softplan.API.Domain.Entities.Task;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Softplan.API.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}
