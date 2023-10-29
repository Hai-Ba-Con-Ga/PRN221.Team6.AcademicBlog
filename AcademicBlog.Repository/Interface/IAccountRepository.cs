using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface IAccountRepository:IBaseRepository<Account>
    {
        Task<IEnumerable<Account>> GetAll();
        Task<Account> Login(string email, string password);
        Task<Account> GetById(int id);
        Task<Account> Add(Account account);
        Task<Account> Update(Account account);
        Task<Account> Delete(int id);
        Task<Account> GetByEmail(string email);
        Task<Account> GetSkillById(int id);
    }
}
