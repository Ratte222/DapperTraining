using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Filters
{
    public class Filter<T>
    {
        public string FieldOrderBy { get; set; }
        public bool OrderByDescending { get; set; } = false;
    }
}
