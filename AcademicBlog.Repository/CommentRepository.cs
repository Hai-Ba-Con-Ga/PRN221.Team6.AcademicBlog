using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;

namespace AcademicBlog.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly GenericDAO<Comment> _commentDAO = new GenericDAO<Comment>();
        public async Task<IEnumerable<Comment>> GetAll(int postId)
        {
            return await _commentDAO.GetAsync(
                filter: x => x.PostId == postId,
                orderBy: x => x.OrderByDescending(y => y.CreatedDate),
                includeProperties: new Expression<Func<Comment, object>>[] { x => x.Post, x => x.Creator });
        }

        public async Task<Comment> Add(Comment comment)
        {
            return await _commentDAO.AddAsync(comment);
        }

        public async Task<Comment> Update(Comment comment)
        {
            return await _commentDAO.UpdateAsync(comment);
        }

        public async Task<Comment> GetById(int id)
        {
            return await _commentDAO.GetByIdAsync(id, includeProperties: new Expression<Func<Comment, object>>[] { x => x.Post, x => x.Creator });
        }
        public void Delete(Comment comment)
        {
            _commentDAO.DeleteAsync(comment);
        }
    }
}
