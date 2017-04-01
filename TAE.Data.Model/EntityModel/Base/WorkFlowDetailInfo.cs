using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class WorkFlowDetailInfo : ComBaseModel
    {
        public string WorkFlowDetailId { get; set; }
        //待审核条目Id（例 项目，财务条目Id)
        public string LinkId { get; set; }
        //业务类型（例 是项目审核 还是 财务审核）
        public int BussType { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public string Idea { get; set; }
        public DateTime? AuditTime { get; set; }
        
    }
}
