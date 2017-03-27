using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    ///  绑定操作通用模型(例:角色(Id)菜单(BindIds)绑定)
    /// </summary>
    public class BindOptionModel
    {
        public string Id { get; set; }
        public string[] BindIds { get; set; }
    }
}
