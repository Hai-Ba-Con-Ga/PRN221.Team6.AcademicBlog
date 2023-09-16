using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
