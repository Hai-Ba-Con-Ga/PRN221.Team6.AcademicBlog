﻿using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AcademicBlog.Repository.Interface
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetList(Pagable pagable);
        Task<(IEnumerable<T>, Pagable)> GetListWithPaging(Pagable pagable);
        Task<IEnumerable<T>> GetList<TProperty>(Pagable pagable, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> Find<TProperty>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include);
        Task<(IEnumerable<T>, Pagable)> GetListWithPaging<TProperty>(Pagable pagable, Func<IQueryable<T>, IIncludableQueryable<T, TProperty>> include);
        Task<Pagable> CountList(Pagable pagable);
        Task<T> GetById(int id);
        Task<T> Add(T post);
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
        Task<T> Update(T post);
        Task Delete(T post);

    }
}
