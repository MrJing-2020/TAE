using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Teacher
{
    /// <summary>
    /// 教师端问答相关
    /// </summary>
    public class QuestionController : TeacherApiController
    {
        /// <summary>
        /// 获取所有被邀请的问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllInvites()
        {
            var teacherId = LoginTeacher.Id;
            string sqlGetAllInvite = @"select * from QuestionAndAnswer where Id in (select TeacherId from InviteAnswer where IsAnswer = 0)";
            SqlParameter param = new SqlParameter("@TeacherId", teacherId);
            var list = ServiceBase.FindBy<QuestionAndAnswer>(sqlGetAllInvite, param).ToList();
            return Response(list);
        }

        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AnswerQuestion(QuestionAndAnswer model)
        {
            var teacherId = LoginTeacher.Id;
            model = ServiceBase.Insert<QuestionAndAnswer>(model);
            InviteAnswer invite = ServiceBase.FindBy<InviteAnswer>(m => m.TeacherId == teacherId && m.QuestionId == model.PreId).FirstOrDefault();
            invite.IsAnswer = true;
            ServiceBase.Update<InviteAnswer>(invite);
            return Response(model);
        }
    }
}
