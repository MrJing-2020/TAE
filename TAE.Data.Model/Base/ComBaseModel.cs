using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 实现数据权限的表需要继承此类(若不继承，则需要自行添加模型中的属性)
    /// </summary>
    public class ComBaseModel : BaseModel
    {
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
    }
}
