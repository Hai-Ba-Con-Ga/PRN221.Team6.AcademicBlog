using AcademicBlog.Domain.Interfaces;
using AcademicBlog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AcademicBlog.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected DbSet<T> dbSet;

    protected AcademicBlogDbContext _context;

    public Repository(AcademicBlogDbContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
    }
    public async Task<int> CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        int rowEffect = await _context.SaveChangesAsync();
        return rowEffect;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        if (entity == null)
        {
            return null;
        }
        dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;

    }

    public async Task<T> DeleteAsync(int id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            return null;
        }
        dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> UpdateAsync(int id, T entity)
    {
        if (entity == null)
        {
            return null;
        }
        var entityUpdate = await dbSet.FindAsync(id);
        if (entityUpdate == null)
        {
            return null;
        }
        _context.Entry(entityUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await dbSet.FindAsync(id);
        return entity;
    }
    public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await dbSet.FirstOrDefaultAsync(expression);
        return entity;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await dbSet.ToListAsync();
        return entities;
    }
    public async Task<IEnumerable<T>> GetMultiByConditionAsync(Expression<Func<T, bool>> expression)
    {
        var entities = await dbSet.Where(expression).ToListAsync();
        return entities;
    }
    public async Task<IEnumerable<T>> GetMultiPagingAsync(Expression<Func<T, bool>> expression, int index = 0, int size = 10)
    {
        var entities = await dbSet.Where(expression).Skip(index * size).Take(size).ToListAsync();
        return entities;
    }
    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        var count = await dbSet.CountAsync(expression);
        return count;
    }
    }
    



