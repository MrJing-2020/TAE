using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 教师
    /// </summary>
    public class TeacherInfo : ComBaseModel
    {
        public string UserId { get; set; }
        public string NickName { get; set; }

        //教师类型（例：语文，数学老师）
        public string TypeId { get; set; }
        public string Introduction { get; set; }
        public bool IsRecommend { get; set; }
        public string PhotoUrl { get; set; }
    }
}
