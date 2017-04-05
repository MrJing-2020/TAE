using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class DepViewModel
    {
        public string Id { get; set; }
        public string DepartName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public bool? IsInAuthority { get; set; }
    }
}
