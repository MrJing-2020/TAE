using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class DataRole:BaseModel
    {
        public string RoleId { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
    }
}
