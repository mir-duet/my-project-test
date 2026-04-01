using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interfaces;
using MyProject.Domain.Entities;
using MyProject.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace MyProject.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync();
        }
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
