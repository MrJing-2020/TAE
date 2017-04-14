using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 班次
    /// </summary>
    public class ClassInfo : BaseModel
    {
        public string Name { get; set; }
        public string TeachingPlanId { get; set; }
        public string CreateCompanyId { get; set; }
        public string Goal { get; set; }
    }
}
