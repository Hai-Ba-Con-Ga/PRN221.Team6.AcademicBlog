using AcademicBlog.BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
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

        //paging and sorting
        public virtual async Task<IEnumerable<T>> GetByPageAsync(int page, int pageSize)
        {
            return await _context.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
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


    }
}
