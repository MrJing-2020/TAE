using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Teacher
{
    public class CourseWareController : BaseApiController
    {
        /// <summary>
        /// 获取我的课件列表
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllCourseWares()
        {
            var list = ServiceBase.FindBy<CourseWare>(m => m.CreateUserId == LoginUser.UserInfo.Id);
            return Response(list);
        }

        /// <summary>
        /// 获取我的课件详情(包括附件信息)
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CourseWaresDetail()
        {
            string sqlCourseWaresDetail = @"";
            var list = ServiceBase.FindBy<CourseWare>(sqlCourseWaresDetail);
            return Response(list);
        }

        [HttpPost]
        public HttpResponseMessage SubCourseWarse(CourseWare model)
        {
            ServiceBase.SaveEntity<CourseWare>(model);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage SubHandouts(Handouts model)
        {
            ServiceBase.SaveEntity<Handouts>(model);
            return Response();
        }
    }
}
