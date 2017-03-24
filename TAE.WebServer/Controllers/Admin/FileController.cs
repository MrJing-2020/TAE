using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.Admin
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using TAE.WebServer.Common;
    using TAE.WebServer.Common.Upload;
    public class FileController : BaseApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            StringBuilder sb = new StringBuilder();
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string savePath = await UploadHelper.uploadFile(Request.Content);
            return Response(savePath);
        }
    }
}
