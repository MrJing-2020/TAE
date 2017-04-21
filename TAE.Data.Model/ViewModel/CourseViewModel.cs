using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class CourseViewModel
    {
        public Course Course { get; set; }
        public List<CourseSection> Section { get; set; }
        public TeacherInfo Teacher { get; set; }
    }
}
