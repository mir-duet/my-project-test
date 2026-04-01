using MyProject.Domain.Entities;
using System.Linq.Expressions;

namespace MyProject.Application.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
}
