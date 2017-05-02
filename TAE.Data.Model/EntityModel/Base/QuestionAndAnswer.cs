using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 问答实体
    /// </summary>
    public class QuestionAndAnswer : BaseModel
    {
        public string MainContent { get; set; }

        //问题相关的课程id
        public string CourseId { get; set; }
        public string UserId { get; set; }

        //true表示是提问，false表示是回答
        public bool IsQuestion { get; set; }
        //父id，例如回答的问题的Id
        public string PreId { get; set; }
    }
}
