using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAE.Data.Model.ViewModel;

namespace TAE.Data.Model
{
    public class DepUserViewModel
    {
        public string Id { get; set; }
        public string DepartName{ get; set; }
        public List<UserAuditViewModel> UserAuditViewModel { get; set; }
    }
}
