using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class BaseModel
    {
        public BaseModel() 
        { 
            IsDel = false;
            CreateTime = DateTime.Now;
        }
        public object Id { get; set; }
        public bool IsDel { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
