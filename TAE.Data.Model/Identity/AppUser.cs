using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TAE.Data.Model
{
    public class AppUser : IdentityUser
    {
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string RealName { get; set; }
    }
}
