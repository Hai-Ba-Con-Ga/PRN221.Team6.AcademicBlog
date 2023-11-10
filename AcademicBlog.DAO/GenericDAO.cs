using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.Extensions;
using AcademicBlog.BussinessObject.PagingObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.DAO
{
    public class GenericDAO <T> where T : class
    {
        protected readonly AcademicBlogDbContext _context;
        private static GenericDAO<T> _instance;
        private static readonly object instanceLock = new object();

        //constructor
        public GenericDAO()
        {
            _context = new AcademicBlogDbContext();
        }
        public GenericDAO(AcademicBlogDbContext context)
        {
            _context = context;
        }

        public static GenericDAO<T> Instance(AcademicBlogDbContext context)
        {
            if (_instance == null)
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new GenericDAO<T>(context);
                    }
                }
            }
            return _instance;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        //get id include
        public virtual async Task<T> GetByIdAsync(int id, Expression<Func<T, object>>[]? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.ToString().Contains(id.ToString()));
        }

        //get id filter and include 
        public virtual async Task<T> GetByIdAsync(int id, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.ToString().Contains(id.ToString()));
        }
        


        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        //include
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync<TProperty>(Pagable paging, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include) 
        {

            IQueryable<T> query = _context.Set<T>();


            query = include(query);

            query = query.ToFilterView(paging);

            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetListAsync(Pagable paging)
        {
            IQueryable<T> query = _context.Set<T>();

            query = query.ToFilterView(paging);

            return await query.ToListAsync();
        }
        public virtual async Task<Pagable> CountListAsync(Pagable paging)
        {
            IQueryable<T> query = _context.Set<T>();
            paging.IsCount = true;
            query = query.ToFilterView(paging);
            int count = query.Count();
            paging.TotalCount = count;
            paging.TotalPage = (int)Math.Ceiling((double)count / paging.PageSize);
            return paging;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Added;
                await _context.Set<T>().AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            return entities;
        }
        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
        public virtual async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        //paging and sorting and include
        
        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Expression<Func<T, object>>[]? includeProperties = null, int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }
        //get all limit order and include
        public virtual async Task<IEnumerable<T>> GetAllAsync(int limit, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Expression<Func<T, object>>[]? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.Take(limit).ToListAsync();
        }


        //search
        public virtual async Task<IEnumerable<T>> GetByKeywordAsync(string keyword)
        {
            return await _context.Set<T>().Where(e => e.ToString().Contains(keyword)).ToListAsync();
        }
        //condition
        public virtual async Task<IEnumerable<T>> GetByConditionAsync(Func<T, bool> expression)
        {
            return await Task.FromResult(_context.Set<T>().Where(expression));
        }

        //get one by condition
        public virtual async Task<T> GetOneByConditionAsync(Func<T, bool> expression)
        {
      
            return await Task.FromResult(_context.Set<T>().Where(expression).FirstOrDefault());
        }

        //get one by condition include
        public virtual async Task<T> GetOneByConditionAsync(Func<T, bool> expression, Expression<Func<T, object>>[]? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await Task.FromResult(query.Where(expression).FirstOrDefault());
        }

        public virtual async Task<IEnumerable<T>> Find<TProperty>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include)
        {
            IQueryable<T> query = _context.Set<T>();
            query = include(query);

            query = query.Where(predicate);
            return query;
        }
        public async virtual  Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>();
            query = query.Where(predicate);
            return query;
        }


    }
}
