using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly GenericDAO<Role> _roleDAO = new GenericDAO<Role>();

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _roleDAO.GetAllAsync();
        }
        public async Task<Role> GetByName(string name)
        {
            return await _roleDAO.GetOneByConditionAsync(x=> x.Name.Equals(name));
        }
        public async Task<Role> GetById(int id)
        {
            return await _roleDAO.GetByIdAsync(id);
        }
        public async Task<Role> Add(Role role)
        {
            return await _roleDAO.AddAsync(role);
        }
        public async Task<Role> Update(Role role)
        {
            return await _roleDAO.UpdateAsync(role);
        }
        public async Task<Role> Delete(int id)
        {
            return await _roleDAO.DeleteAsync(id);
        }

    }
}
