using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;

namespace AcademicBlog.Repository
{
    public interface IBookmarkRepository
    {
        Task<IEnumerable<Bookmark>> GetAll(int creatorId, int page, int pageSize);
        Task<IEnumerable<Bookmark>> GetAll(int creatorId, string keyword, int page, int pageSize);
        Task<Bookmark> Add(int creatorId, int postId);
        Task<Bookmark> Delete(int creatorId, int postId);

    }
}
