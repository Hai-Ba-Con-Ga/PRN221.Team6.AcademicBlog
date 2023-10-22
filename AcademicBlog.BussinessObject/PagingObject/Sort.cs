using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.BussinessObject.PagingObject
{
    public class Sort
    {
       
            public string Field { get; set; }
            public string Dir { get; set; }
        
    }
    public static class SortDirection
    {
        public const string ASC = "ASC";
        public const string DESC = "DESC";
    }
}
