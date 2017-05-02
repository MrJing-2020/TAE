using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// post方法无法将json格式的参数转成单字符串，用OneParam model 代替 string id
    /// </summary>
    public class OneParam
    {
        public string Id { get; set; }
    }
}
