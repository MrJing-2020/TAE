using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class BaseModel
    {
        public BaseModel() 
        { 
            IsDel = false;
            LastModifiedTime = DateTime.Now;
        }
        [Key]
        public string Id { get; set; }
        public bool IsDel { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
