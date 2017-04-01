using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class LoginUser
    {
        public AppUser UserInfo { get; set; }
        public List<DataRole> DataPower { get; set; }
    }
}
