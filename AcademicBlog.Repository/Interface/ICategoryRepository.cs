using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface ICategoryRepository:IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetAll();
    }
}
