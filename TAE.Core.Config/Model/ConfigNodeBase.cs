using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Core.Config
{
    /// <summary>
    /// Config节点
    /// </summary>
    public class ConfigNodeBase
    {
        public ConfigNodeBase() { }

        public int Id { get; set; }

        public int Order { get; set; }
    }
}
