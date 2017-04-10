using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 用户相关数据
    /// </summary>
    public class UserViewModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string RealName { get; set; }
        public string CompanyName { get; set; }
        public string DepartName { get; set; }
        public string PositionName { get; set; }

    }
}
