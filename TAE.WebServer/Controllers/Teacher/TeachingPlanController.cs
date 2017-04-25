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
    /// <summary>
    /// 办班计划（教学大纲），班次
    /// </summary>
    public class TeachingPlanController : BaseApiController
    {
        /// <summary>
        /// 获取所有办班计划（教学大纲）
        /// </summary>
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

        /// <summary>
        /// 根据办班计划（教学大纲）获取班次
        /// </summary>
        /// <param name="id">TeachingPlan Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllClass(string id)
        {
            var list = ServiceBase.FindBy<ClassInfo>(m => m.TeachingPlanId == id);
            return Response(list);
        }

        [HttpPost]
        public HttpResponseMessage SubTeachingPlanData(TeachingPlan model)
        {
            ServiceBase.SaveEntity<TeachingPlan>(model);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage SubClassInfoData(ClassInfo model)
        {
            ServiceBase.SaveEntity<ClassInfo>(model);
            return Response();
        }

        /// <summary>
        /// 将班次通知到下属公司
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostClassNotification(BindOptionModel model)
        {
            var classId = model.Id;
            List<Notification> list = new List<Notification>();
            foreach (var item in model.BindIds)
            {
                var noti = new Notification
                {
                    ClassInfoId = classId,
                    CompanyId = item
                };
            }
            ServiceBase.Insert<Notification>(list);
            return Response();
        }
    }
}
