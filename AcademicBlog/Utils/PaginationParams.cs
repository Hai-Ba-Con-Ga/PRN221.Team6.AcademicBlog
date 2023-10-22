using Microsoft.AspNetCore.Mvc;

namespace AcademicBlog.Utils
{
    public class PaginationParams
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        public int Total { get; set; }
        public int PageCount { get; set; }
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 3;
    }
}
