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
        public int CompId { get; set; }
        public int DepId { get; set; }
        public int Type { get; set; }
        public List<WorkFlowDetail> WorkFlowDetail { get; set; }
    }
}
