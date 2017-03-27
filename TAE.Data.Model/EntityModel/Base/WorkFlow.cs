using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class WorkFlow : BaseModel
    {
        [Key]
        public new int Id { get; set; }
        public string Name { get; set; }
        public int CompId { get; set; }
        public int DepId { get; set; }
        public int Type { get; set; }
    }
}
