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
        [Key]
        public new int Id { get; set; }
        public string RoleId { get; set; }
        public int MenuId { get; set; }
    }
}
