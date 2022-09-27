using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IRepository<T> : IDisposable
        where T : class, IBaseEntity
    {
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        IQueryable<T> FindAll();

        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}