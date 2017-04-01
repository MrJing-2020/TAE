using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    using System.ComponentModel.DataAnnotations;
    public class MenuRole:BaseModel
    {
        public string RoleId { get; set; }
        public string MenuId { get; set; }
    }
}
