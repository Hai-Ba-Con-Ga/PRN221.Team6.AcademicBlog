using AcademicBlog.BussinessObject.PagingObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.BussinessObject.PagingObject
{
    public class Pagable
    {
        public Pagable() { }
        public int PageIndex { get; set; } = 1;
        public int TotalCount { get; set; } = 20;

        public int PageSize { get; set; }
        public int TotalPage { get; set; }

        public bool? IsCount { get; set; } = false;
        public IEnumerable<Sort> Sort { get; set; }
        public Filter Filter { get; set; }


    }
}
