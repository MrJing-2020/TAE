using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class FlowViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartName { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public List<FlowDetailViewModel> WorkFlowDetail { get; set; }
    }
}
