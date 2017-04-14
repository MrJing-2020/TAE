using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class DocMain : BaseModel
    {
        public string ApiId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string SupportType { get; set; }
        public string RequestParms { get; set; }
        public string ReturnData { get; set; }
        public string ErrorResult { get; set; }
    }
}
