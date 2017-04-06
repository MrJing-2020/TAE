using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// jstree数据显示model，属性为小写是为配合jstree
    /// </summary>
    public class TreeModelView
    {
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public List<TreeModelView> children { get; set; }

    }
}
