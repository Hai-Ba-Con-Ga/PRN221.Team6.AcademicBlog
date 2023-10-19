using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface IPostTagRepository
    {
        Task<IEnumerable<PostTag>> GetAll();
        Task<PostTag> GetByName(string name);
        Task<PostTag> GetById(int id);
        Task<PostTag> Add(PostTag postTag);
        Task<PostTag> Update(PostTag postTag);
        void Delete(int id);
        void Delete(PostTag postTag);
    }
}
