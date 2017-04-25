using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Stutent
{
    /// <summary>
    /// 学员端问答相关
    /// </summary>
    public class QuestionController : BaseApiController
    {
        /// <summary>
        /// 获取问题（包括回答）
        /// </summary>
        /// <param name="onlyAboutMy"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllMyQuestion(bool onlyAboutMy = false)
        {
            List<QuestionAndAnswer> list = new List<QuestionAndAnswer>();
            List<QuestionViewModel> listResult = new List<QuestionViewModel>();
            if (onlyAboutMy == false)
            {
                list = ServiceBase.FindBy<QuestionAndAnswer>().ToList();
            }
            else
            {
                list = ServiceBase.FindBy<QuestionAndAnswer>(m=>m.UserId==LoginUser.UserInfo.Id).ToList();
            }
            foreach (var item in list)
            {
                QuestionViewModel question = new QuestionViewModel
                {
                    Question = item
                };
                question.Answer = new List<QuestionAndAnswer>();
                foreach (var item1 in list.Where(m => m.PreId == item.Id))
                {
                    question.Answer.Add(item1);
                }
                listResult.Add(question);
            }
            return Response(listResult);
        }

        /// <summary>
        /// 提问或回答问题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AskOrAnswerQue(QuestionAndAnswer model)
        {
            model.UserId = LoginUser.UserInfo.Id;
            var result = ServiceBase.Insert<QuestionAndAnswer>(model);
            return Response(result);
        }

        /// <summary>
        /// 获取教师列表（可根据教师类型筛选）
        /// </summary>
        /// <param name="id">typeId</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTeachers(string id = "")
        {
            List<TeacherInfo> list = new List<TeacherInfo>();
            if (id == "")
            {
                list = ServiceBase.FindBy<TeacherInfo>().ToList();
            }
            else
            {
                list = ServiceBase.FindBy<TeacherInfo>(m => m.TypeId == id).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 邀请教师回答问题
        /// </summary>
        /// <param name="model">Id:问题id，BindIds：教师id</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InviteAnswer(BindOptionModel model)
        {
            string questionId = model.Id;
            List<InviteAnswer> list = new List<InviteAnswer>();
            foreach (var item in model.BindIds)
            {
                InviteAnswer invite = new InviteAnswer
                {
                    QuestionId = questionId,
                    TeacherId = item,
                    IsAnswer = false
                };
                list.Add(invite);
            }
            ServiceBase.Insert<InviteAnswer>(list);
            return Response();
        }
    }
}
