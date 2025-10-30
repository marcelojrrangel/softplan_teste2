using System.Threading.Tasks;

namespace Softplan.API.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}