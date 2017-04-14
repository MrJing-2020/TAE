using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class CourseAccBaseModel : BaseModel
    {
        public string CourseWareId { get; set; }
        public string CreateUserId { get; set; }
        public bool IsPublic { get; set; }
    }
}
