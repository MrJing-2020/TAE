using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.IService
{
    using TAE.Data.Model;

    public interface IServiceBase : IServiceExtend
    {
        bool DelFile(string id);
        bool DelFile(FilesInfo model);
    }
}
