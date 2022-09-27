using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories.Base;

internal class Repository<T> : IRepository<T>
    where T : class, IBaseEntity
{
    protected readonly TicketManagementContext Context;
    protected readonly DbSet<T> Table;

    private protected Repository(TicketManagementContext context)
    {
        Context = context;
        Table = Context.Set<T>();
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        var result = Table.Where(predicate);
        if (includes.Any())
        {
            result = includes
                .Aggregate(result,
                    (current, include)
                        => current.Include(include));
        }

        return result;
    }

    public IQueryable<T> FindAll()
    {
        return Table;
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Table.FirstOrDefaultAsync(item => item.Id.Equals(id), cancellationToken);
    }

    public void Add(T entity)
    {
        Table.Add(entity);
    }

    protected void AddRange(IEnumerable<T> elements)
    {
        Table.AddRangeAsync(elements);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> elements)
    {
        Table.UpdateRange(elements);
    }

    public void Remove(T entity)
    {
        Table.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> elements)
    {
        Table.RemoveRange(elements);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Context?.Dispose();
    }
}
