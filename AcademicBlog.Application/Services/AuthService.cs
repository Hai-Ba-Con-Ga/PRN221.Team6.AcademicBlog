using AcademicBlog.Domain.Interfaces;
using AcademicBlog.Domain.Models;
using AcademicBlog.Domain.Interfaces.Services;
using AcademicBlog.Domain.Entities;

namespace AcademicBlog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Role> _roleRepository; 

        public AuthService(IRepository<Account> accountRepository, IRepository<Role> roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task Delete(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            await _accountRepository.DeleteAsync(account);
           
        }

        public async Task<Account> Login(string email, string password)
        {
            var account = await _accountRepository.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (account == null)
            {
                throw new Exception("Email or password is incorrect");
            }
            return account;

        }

        public async Task<Account> Register(SignupModel model)
        {
            var account = await _accountRepository.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (account != null)
            {
                throw new Exception("Email is already in use");
            }
            account = new Account
            {
                Email = model.Email,
                Password = model.Password,
                Fullname = model.Fullname,
                RoleId = (await _roleRepository.FirstOrDefaultAsync(x => x.Name == "User")).Id
            };
            await _accountRepository.InsertAsync(account);
            return account;
            
        }

        public async Task<Account> Update(int id, UpdateAccountModel model)
        {
            var account = _accountRepository.Find(id);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            account.Fullname = model.Fullname;
            account.Password = model.Password;
            account.AvatarUrl = model.AvatarUrl;
            await _accountRepository.UpdateAsync(account);
            return account;

        }

        
    }
}
