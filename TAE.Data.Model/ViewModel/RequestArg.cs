using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 请求参数模型
    /// </summary>
    public class RequestArg
    {
        public const int defualtPageSize = 10;
        public RequestArg()
        {
            PageNumber = 1;
            PageSize = defualtPageSize;
            IsAsc = true;
        }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsAsc { get; set; }
    }
}
