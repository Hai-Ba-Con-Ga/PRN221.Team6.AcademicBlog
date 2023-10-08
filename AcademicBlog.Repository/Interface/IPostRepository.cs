using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAll(int limit);
        Task<IEnumerable<Post>> GetAll(int page, int pageSize);
        Task<IEnumerable<Post>> GetAllFront(int page, int pageSize);
        Task<IEnumerable<Post>> GetAllFrontByName(int page, int pageSize, string name);
        Task<Post> GetById(int id);
        Task<Post> Add(Post post);
        Task<Post> Update(Post post);
        void Delete(Post post);
        
    }
}