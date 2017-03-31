using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class WorkFlowDetail : BaseModel
    {
        [Key]
        public new int Id { get; set; }
        public int WorkFlowId { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public string DefualtAuditUserId { get; set; }
    }
}
