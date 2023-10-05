using AcademicBlog.Domain.Interfaces;
using AcademicBlog.Domain.Models;
using AcademicBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.Domain.Interfaces.Services;

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

        public async Task<Account> Login(string email, string password)
        {
            var account = await _accountRepository.FindAsync(email, password);
            return account;
        }

        public async Task<Account> Register(string email, string password, string fullname)
        {
            //var role = await _roleRepository.GetAllAsync();
            var checkAccount = await _accountRepository.FindAsync(email);
            if (checkAccount != null)
            {
                throw new Exception("Email đã tồn tại");
            }

            var account = new Account
            {
                Email = email,
                Password = password,
                Fullname = fullname,
                RoleId = 2
            };
            await _accountRepository.InsertAsync(account);
            return account;
        }

        public async Task<Account> Update(int id, UpdateAccountModel model)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            
            account.Password = model.Password;
            account.Fullname = model.Fullname;
            account.AvatarUrl = model.AvatarUrl;
            
            await _accountRepository.UpdateAsync(account);
            return account;
        }

        public async Task Delete(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            await _accountRepository.DeleteAsync(account);
        }

        Task<Account> IAuthService.Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        Task<Account> IAuthService.Register(string email, string password, string fullname)
        {
            throw new NotImplementedException();
        }

        Task<Account> IAuthService.Update(int id, UpdateAccountModel model)
        {
            throw new NotImplementedException();
        }
    }
}
