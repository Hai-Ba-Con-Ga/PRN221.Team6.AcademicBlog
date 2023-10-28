using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly GenericDAO<T> _genericDAO = new GenericDAO<T>();
        public Task<T> Add(T post)
        {
            return _genericDAO.AddAsync(post);
        }

        
        public Task Delete(T post)
        {
            return _genericDAO.DeleteAsync(post);

        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetList(Pagable pagable)
        {
           return  _genericDAO.GetListAsync(pagable);
        }
        public Task<IEnumerable<T>> GetList<TProperty>(Pagable pagable, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include)
        {
            return _genericDAO.GetListAsync(pagable, include);
        }

        public async Task<(IEnumerable<T>, Pagable)> GetListWithPaging(Pagable pagable)
        {
            var list = await GetList(pagable);
            var paging = await CountList(pagable);
            return (list, paging);
        }
        public async Task<(IEnumerable<T>, Pagable)> GetListWithPaging<TProperty>(Pagable pagable,Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include)
        {
            var list = await GetList(pagable, include);
            var paging = await CountList(pagable);
            return (list, paging);
        }
        public async Task<Pagable> CountList(Pagable pagable)
        {
            var countObj = await _genericDAO.CountListAsync(pagable);
            pagable.TotalCount = countObj.TotalCount;
            pagable.TotalPage = countObj.TotalPage;
            return pagable;
        }


        public Task<T> Update(T post)
        {
            return _genericDAO.UpdateAsync(post);
        }

        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return _genericDAO.Find(predicate);
        }

        public Task<IEnumerable<T>> Find<TProperty>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include)
        {
             return _genericDAO.Find(predicate, include);
        }
    }
}
