using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class QuestionViewModel
    {
        public QuestionAndAnswer Question { get; set; }
        public List<QuestionAndAnswer> Answer { get; set; }
        public int AnswerCount { get; set; }
    }
}
