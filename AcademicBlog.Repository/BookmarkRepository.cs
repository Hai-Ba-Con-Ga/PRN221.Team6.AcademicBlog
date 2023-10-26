using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;

namespace AcademicBlog.Repository
{
    public class BookmarkRepository : BaseRepository<Bookmark>, IBookmarkRepository
    {
        private readonly GenericDAO<Bookmark> _bookmarkDAO = new GenericDAO<Bookmark>();

        
        //get by creator
        public async Task<IEnumerable<Bookmark>> GetAll(int creatorId, int page, int pageSize)
        {
            return await _bookmarkDAO.GetAsync(
                page: page,
                pageSize: pageSize,
                filter: x => x.Post.IsPublic == true && x.CreatorId == creatorId,
                orderBy: x => x.OrderByDescending(y => y.Post.CreatedDate),
                includeProperties: new Expression<Func<Bookmark, object>>[] { x => x.Post, x => x.Creator });
        }
        public async Task<IEnumerable<Bookmark>> GetAll(int creatorId, string keyword, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await GetAll(creatorId, page, pageSize);
            }
            return await _bookmarkDAO.GetAsync(
                page: page,
                pageSize: pageSize,
                filter: x => x.Post.IsPublic == true && x.CreatorId == creatorId && x.Post.Title.Contains(keyword),
                orderBy: x => x.OrderByDescending(y => y.Post.CreatedDate),
                includeProperties: new Expression<Func<Bookmark, object>>[] { x => x.Post, x => x.Creator });
        }


        //post  bookmark
        public async Task<Bookmark> Add(int creatorId, int postId)
        {
            var bookmark = new Bookmark
            {
                CreatorId = creatorId,
                PostId = postId
            };
            return await _bookmarkDAO.AddAsync(bookmark);
        }

        //delete bookmark
        public async Task<Bookmark> Delete(int creatorId, int postId)
        {
            var bookmark = await _bookmarkDAO.GetOneByConditionAsync(x => x.CreatorId == creatorId && x.PostId == postId);
            return await _bookmarkDAO.DeleteAsync(bookmark);
        }
    
    }
}
