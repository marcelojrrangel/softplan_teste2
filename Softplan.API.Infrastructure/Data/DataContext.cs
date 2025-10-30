using Microsoft.EntityFrameworkCore;
using TaskEntity = Softplan.API.Domain.Entities.Task;

namespace Softplan.API.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<TaskEntity> Tasks { get; set; }
}
