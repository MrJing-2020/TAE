using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class PageList<T> where T : class
    {
        public IQueryable<T> DataList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
