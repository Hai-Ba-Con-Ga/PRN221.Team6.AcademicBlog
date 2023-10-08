using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAll();
        Task<Role> GetByName(string name);
        Task<Role> GetById(int id);
        Task<Role> Add(Role role);
        Task<Role> Update(Role role);
        Task<Role> Delete(int id);
    }
}
