using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class WorkFlowDetail : ComBaseModel
    {
        public string WorkFlowId { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public string DefualtAuditUserId { get; set; }
        public int AuditUserCount { get; set; }
    }
}
