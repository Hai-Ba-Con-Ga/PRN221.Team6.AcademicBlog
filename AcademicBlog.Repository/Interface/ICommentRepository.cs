using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;

namespace AcademicBlog.Repository.Interface
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll(int postId);
        Task<Comment> Add(Comment comment);
        Task<Comment> Update(Comment comment);
        Task<Comment> GetById(int id);
        void Delete(Comment comment);    
    }
}
