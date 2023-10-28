using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface ISkillRepository : IBaseRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetAll();
        Task<IEnumerable<Skill>> GetByIds(List<int> ids);

    }
}
