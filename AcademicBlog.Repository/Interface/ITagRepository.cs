using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface ITagRepository
    {
        public  Task<Tag> FindByName(string name);
        public  Task<Tag> Add(Tag tag);
    }
}
