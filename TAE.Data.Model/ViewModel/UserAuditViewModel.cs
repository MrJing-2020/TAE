using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model.ViewModel
{
   public class UserAuditViewModel
    {
       public string Id { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string RealName { get; set; }
        public bool IsAudit { get; set; }
    }
}
