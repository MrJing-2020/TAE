using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class Company:BaseModel
    {
        public string CompanyName { get; set; }
        //公司级别
        public int Grade { get; set; }
        public string PreCompanyId { get; set; }
        public string LegalPersonName { get; set; }
        public string LinkPersonName { get; set; }
        public string PhoneNumber { get; set; }
        public int BusinessType { get; set; }
        public string BusinessRange { get; set; }
        public string Address { get; set; }
        public string CompanyLogo { get; set; }
        //公司性质： 例子公司，分公司
        public int CompanyNature { get; set; }

    }
}
