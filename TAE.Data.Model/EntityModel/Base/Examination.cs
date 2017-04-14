using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class Examination : BaseModel
    {
        public string Name { get; set; }
        public string CourseId { get; set; }
    }
}
