using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class InviteAnswer : BaseModel
    {
        public string QuestionId { get; set; }
        public string TeacherId { get; set; }
        //是否已经回答
        public bool IsAnswer { get; set; }
    }
}
