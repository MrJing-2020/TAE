using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Utility.Common
{
    /// <summary>
    /// 请求参数模型
    /// </summary>
    public class RequestArg
    {
        public RequestArg()
        {
            pageNumber = 1;
            pageSize = 10;
            total = 0;
        }
        public string Name { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
    }
}
