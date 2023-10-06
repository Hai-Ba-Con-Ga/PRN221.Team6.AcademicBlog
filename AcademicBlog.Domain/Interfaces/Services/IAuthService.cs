using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.Domain.Entities;
using AcademicBlog.Domain.Models;

namespace AcademicBlog.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Account> Login(string email, string password);
        Task<Account> Register(SignupModel signupModel);
        Task<Account> Update(int id, UpdateAccountModel model);
        Task Delete(int id);
        
    }
}
