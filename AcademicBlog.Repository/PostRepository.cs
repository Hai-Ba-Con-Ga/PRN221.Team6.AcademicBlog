using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly GenericDAO<Post> _postDAO = new GenericDAO<Post>();
        private readonly GenericDAO<Comment> _commnetDAO = new GenericDAO<Comment>();
        private readonly GenericDAO<PostTag> _postTagDAO = new GenericDAO<PostTag>();
        private readonly GenericDAO<Tag> _tagDAO = new GenericDAO<Tag>();

        public async Task<IEnumerable<Post>> GetAll(int limit)
        {
            //get limit
            return await _postDAO.GetAllAsync(
                limit: limit,
                filter: x => x.IsPublic == true,
                orderBy: x => x.OrderByDescending(y => y.CreatedDate),
                includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
        }

        //get all with paging and order by
        public async Task<IEnumerable<Post>> GetAllFront(int page, int pageSize)
        {
            return await _postDAO.GetAsync(
                page: page,
                pageSize: pageSize,
                filter: x => x.IsPublic == true,
                orderBy: x => x.OrderByDescending(y => y.CreatedDate),
                includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
            
        }

        //get all with paging and order by with search tag
        // public async Task<IEnumerable<Post>> GetAllFrontByTag(int page, int pageSize, string tag)
        // {
        //     return await _postDAO.GetAsync(
        //         page: page,
        //         pageSize: pageSize,
        //         filter: x => x.IsPublic == true && x.PostTags.Any(y => y.Tag.Name.Contains(tag)),
        //         orderBy: x => x.OrderByDescending(y => y.CreatedDate),
        //         includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
            
        // }
        

        //get search by name
        public async Task<IEnumerable<Post>> GetAllFrontByName(int page, int pageSize, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await GetAllFront(page, pageSize);
            }
            return await _postDAO.GetAsync(
                page: page,
                pageSize: pageSize,
                filter: x => x.IsPublic == true && x.Title.Contains(keyword),
                orderBy: x => x.OrderByDescending(y => y.CreatedDate),
                includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
            
        }

        public async Task<IEnumerable<Post>> GetAll(int page, int pageSize)
        {
            return await _postDAO.GetAsync(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.OrderByDescending(y => y.CreatedDate),
                includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
            
        }
        
        //get id include
        public async Task<Post?> GetById(int id)
        {
            var post = (await _postDAO.Find(post => post.Id == id,  t => t.Include(i => i.Comments).ThenInclude(c=>c.Creator).Include(p=>p.Category).Include(p => p.Creator).Include(p => p.Skills).Include(p => p.Tags))).FirstOrDefault();
           
            return post;
        }

        //get id include for front
        public async Task<Post?> GetByIdFront(int id)
        {
            return await _postDAO.GetByIdAsync(id,
            filter: x => x.IsPublic == true,
            includeProperties: new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator });
            //return (await _postDAO.Find(post => post.IsPublic == true && post.Id == id, new Expression<Func<Post, object>>[] { x => x.Category, x => x.Creator })).FirstOrDefault();
        }



        public async Task<Post> Add(Post post)
        {
            return await _postDAO.AddAsync(post);
        }
        public async Task<Post> Update(Post post)
        {
            return await _postDAO.UpdateAsync(post);
        }
        public void Delete(Post post)
        {
            _postDAO.DeleteAsync(post);
        }

        public  async Task<IEnumerable<Post>> GetAllPost(Pagable pagable)
        {
            var list = await _postDAO.GetListAsync(pagable, t => t.Include(i => i.Comments).ThenInclude(c=>c.Creator).Include(p => p.Category).Include(p => p.Creator).Include(p => p.Skills));
            return list;
        }
        public async Task<Pagable> CountList(Pagable pagable)
        {
            var countObj = await _postDAO.CountListAsync(pagable);
            pagable.TotalCount = countObj.TotalCount;
            pagable.TotalPage = countObj.TotalPage;
            return pagable;
        }
       
    }
}
