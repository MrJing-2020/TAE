using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Admin
{
    /// <summary>
    /// 办班计划（教学大纲）
    /// </summary>
    public class TeachingPlanController : BaseApiController
    {
        /// <summary>
        /// 获取所有办班计划
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllTeachingPlans(bool onlyOurCompany = false)
        {
            List<TeachingPlan> list = new List<TeachingPlan>();
            if (onlyOurCompany == false)
            {
                list = ServiceBase.FindBy<TeachingPlan>().ToList();
            }
            else
            {
                list = ServiceBase.FindBy<TeachingPlan>(m => m.CompanyId == LoginUser.UserInfo.CompanyId).ToList();
            }
            return Response(list);
        }

        [HttpPost]
        public HttpResponseMessage SubTeachingPlan(TeachingPlan model)
        {
            ServiceBase.SaveEntity<TeachingPlan>(model);
            return Response();
        }
    }
}
