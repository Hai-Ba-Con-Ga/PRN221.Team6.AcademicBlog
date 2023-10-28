using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly GenericDAO<Account> _accountDAO = new GenericDAO<Account>();

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _accountDAO.GetAllAsync();
        }
        public async Task<Account> GetById(int id)
        {
            return await _accountDAO.GetByIdAsync(id);
        }

        public async Task<Account> GetSkillById(int id)
        {
            return await _accountDAO.GetOneByConditionAsync(

                    expression: x => x.Id == id,
                    includeProperties: new Expression<Func<Account, object>>[] { x => x.Skills }
                );
        }
        public async Task<Account> Login(string email, string password)
        {
            return await _accountDAO.GetOneByConditionAsync(

                    expression: x => x.Email == email && x.Password == password,
                    includeProperties: new Expression<Func<Account, object>>[] { x => x.Role }
                );
        }

        public async Task<Account> Add(Account account)
        {
            return await _accountDAO.AddAsync(account);
        }
        public async Task<Account> Update(Account account)
        {
            return await _accountDAO.UpdateAsync(account);
        }
        public async Task<Account> Delete(int id)
        {
            return await _accountDAO.DeleteAsync(id);
        }
        public async Task<Account> GetByEmail(string email)
        {
            return await _accountDAO.GetOneByConditionAsync(

                    expression: x => x.Email == email,
                    includeProperties: new Expression<Func<Account, object>>[] { x => x.Role }
                );
        }

    }
}
