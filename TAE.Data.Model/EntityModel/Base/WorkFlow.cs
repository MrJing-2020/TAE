using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class WorkFlow : ComBaseModel
    {
        public string Name { get; set; }

        //Type属性与Type表中的Id关联
        public string Type { get; set; }
    }
}
