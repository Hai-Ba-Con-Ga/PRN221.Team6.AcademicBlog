using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.BussinessObject.PagingObject
{
    public static class FilterOps
    {
        public const string EQUAL = "eq";  // Equals
        public const string NOT_EQUAL = "neq";  // Not Equals
        public const string LESS_THAN = "lt";  // Less Than
        public const string LESS_THAN_OR_EQUAL = "lte";  // Less Than or Equal
        public const string GREATER_THAN = "gt";  // Greater Than
        public const string GREATER_THAN_OR_EQUAL = "gte";  // Greater Than or Equal
        public const string STARTS_WITH = "startswith";  // Starts With
        public const string ENDS_WITH = "endswith";  // Ends With
        public const string CONTAINS = "contains";  // Contains
        public const string DOES_NOT_CONTAIN = "doesnotcontain";  // Does Not Contain
        public const string IN = "in";  // In
    }
    public static class FilterLogic
    {
        public const string AND = "and";
        public const string OR = "or";
    }
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
    }
}
