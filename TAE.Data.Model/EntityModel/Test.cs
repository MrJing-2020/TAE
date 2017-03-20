using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class Test:BaseModel
    {
        [Key]
        public new int Id { get; set; }
        public string Name { get; set; }

    }
}
