using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Models
{
    public class SignupModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
    }
}
