using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class CourseWareViewModel
    {
        public CourseWare CourseWare { get; set; }
        public List<Handouts> Handouts { get; set; }
        public List<TestQuestion> TestQuestion { get; set; }
        public List<TestPaper> TestPaper { get; set; }
        public List<Video> Video { get; set; }
        public List<PowerPoint> PowerPoint { get; set; }
    }
}
