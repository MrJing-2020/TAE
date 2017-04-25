using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class StudentInfo : ComBaseModel
    {
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
