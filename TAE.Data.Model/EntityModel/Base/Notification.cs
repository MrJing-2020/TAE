using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 班次发放通知，可通知到公司，部门，人员
    /// </summary>
    public class Notification : BaseModel
    {
        public string ClassInfoId { get; set; }
        public string NotificationContent { get; set; }
        public string CompanyId { get; set; }
        public string DepartmantId { get; set; }
        public string UserId { get; set; }
    }
}
