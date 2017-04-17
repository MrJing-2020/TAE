using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.ApiDoc
{
    using TAE.Data.Model;
    using TAE.WebServer.Common;

    public class DocumentController : BaseApiController
    {
        /// <summary>
        ///接口显示
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetApiDocList()
        {
            var list = ServiceApiDoc.FindBy<DocMain>().ToList();
            return Response(list);
        }

        /// <summary>
        /// 提交新增和编辑Api数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubDocData(DocMain model)
        {
            ServiceApiDoc.SaveEntity<DocMain>(model);
            return Response();
        }
        /// <summary>
        ///删除数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelDocData(string ID)
        {
            ServiceApiDoc.Remove<DocMain>(s => s.Id == ID);
            return Response();
        }
        /// <summary>
        ///查看详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDocDetail(string ID)
        {
           var list = ServiceApiDoc.FindBy<DocMain>(s => s.Id == ID).FirstOrDefault();
            return Response(list);
        }
        /// <summary>
        ///接口分类显示
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetApiData()
        {
            var list = ServiceApiDoc.FindBy<OpenApi>();
            return Response(list);
        }
        /// <summary>
        ///接口分类添加和修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubApiData(OpenApi model)
        {
            ServiceApiDoc.SaveEntity<OpenApi>(model);
            return Response();
        }
        /// <summary>
        ///接口分类删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelApiData(string ID)
        {
            ServiceApiDoc.Remove<OpenApi>(s=>s.Id==ID);
            return Response();
        }
        /// <summary>
        ///接口分类详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetApiDetail(string ID)
        {
            var list = ServiceApiDoc.FindBy<DocMain>(s => s.ApiId == ID).ToList();
            return Response(list);
        }
    }
}
