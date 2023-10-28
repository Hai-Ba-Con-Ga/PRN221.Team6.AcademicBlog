using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;

namespace AcademicBlog.Repository
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public readonly GenericDAO<Skill> _skillDAO = new GenericDAO<Skill>();

        //get all skill
        public async Task<IEnumerable<Skill>> GetAll(){
            return await _skillDAO.GetAllAsync();
        }
        public async Task<IEnumerable<Skill>> GetByIds(List<int> ids){
            return await _skillDAO.GetAsync(x => ids.Contains(x.Id));
        }
    }
}
