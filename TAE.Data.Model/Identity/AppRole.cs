using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAE.Data.Model
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        public string Description { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
    }
}
