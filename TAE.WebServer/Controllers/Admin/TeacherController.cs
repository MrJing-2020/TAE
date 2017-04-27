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
    public class TeacherController : BaseApiController
    {
        /// <summary>
        /// 获取教师（提供多字段索引，排序）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AllTeachers(dynamic param)
        {
            string sqlGetAll = @"select a.* from TeacherInfo as a";
            return GetDataList<TeacherInfo>(param, sqlGetAll);
        }

        /// <summary>
        /// 获取所有教师（未分页）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetAllTeachers()
        {
            var list = ServiceBase.FindBy<TeacherInfo>();
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage TeacherBind(TeacherBind model)
        {
            AppUser user = ServiceIdentity.FindUser(m => m.Id == model.UserId).FirstOrDefault();
            TeacherInfo teacher = new TeacherInfo
            {
                UserId = model.UserId,
                TypeId = model.TypeId,
                CompanyId = user.CompanyId,
                DepartmentId = user.DepartmentId,
                NickName = "默认昵称"
            };
            return Response();
        }

        /// <summary>
        /// 设置推荐教师（推荐教师将在首页列出）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage TeacherRecommend(OneParamList model)
        {
            //先把所有IsRecommend设置为False
            var teacherList = ServiceBase.FindBy<TeacherInfo>().ToList();
            foreach (var item in teacherList)
            {
                item.IsRecommend = false;
            }
            List<TeacherInfo> updateTeachers = new List<TeacherInfo>();
            foreach (var item in model.Keys)
            {
                TeacherInfo teacher= teacherList.Where(m => m.Id == item).FirstOrDefault();
                teacher.IsRecommend = true;
                updateTeachers.Add(teacher);
            }
            ServiceBase.Update<TeacherInfo>(teacherList);
            ServiceBase.Update<TeacherInfo>(updateTeachers);
            return Response();
        }


        [HttpGet]
        public HttpResponseMessage TeacherTypeSelect()
        {
            int teacherType = Convert.ToInt32(TypeEnum.Teacher);
            string sql = "select Id as 'Key',TypeName as 'Value' from Type where TypeGroup = " + teacherType;
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql).ToList();
            return Response(list);
        }
    }
}
