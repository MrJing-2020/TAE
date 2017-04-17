using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class Type : BaseModel
    {
        //类型名
        public string TypeName { get; set; }

        //分类(例：流程类型，文件类型)
        public int TypeGroup { get; set; }
    }
}
