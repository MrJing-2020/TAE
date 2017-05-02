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
        public HttpResponseMessage AllMyQuestion(int pageNumber = 1, int pageSize = 10, bool onlyAboutMy = false)
        {
            RequestArg arg = new RequestArg()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            PageList<QuestionAndAnswer> pageList = new PageList<QuestionAndAnswer>();
            if (onlyAboutMy == false)
            {
                pageList = ServiceBase.FindAllByPage<QuestionAndAnswer>(m => m.IsQuestion == true, arg);
            }
            else
            {
                pageList = ServiceBase.FindAllByPage<QuestionAndAnswer>(m => m.UserId == LoginUser.UserInfo.Id && m.IsQuestion == true, arg);
            }
            PageList<QuestionViewModel> pageListResult = new PageList<QuestionViewModel>
            {
                PageNumber = pageList.PageNumber,
                PageSize = pageList.PageSize,
                Total = pageList.Total
            };
            List<QuestionAndAnswer> list = pageList.DataList.ToList();
            List<QuestionViewModel> listDataResult = new List<QuestionViewModel>();
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
                question.AnswerCount = question.Answer.Count();
                listDataResult.Add(question);
            }

            pageListResult.DataList = listDataResult.AsQueryable<QuestionViewModel>();
            return Response(pageListResult);
        }

        /// <summary>
        /// 获取问题详情（问题，回答，回答的回复数量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns>question:问题, answer:回答列表, answerCount:回答数量</returns>
        [HttpGet]
        public HttpResponseMessage GetQuestionDetail(string id)
        {
            var question = ServiceBase.FindBy<QuestionAndAnswer>(m => m.Id == id).FirstOrDefault();
            var list = ServiceBase.FindBy<QuestionAndAnswer>(m => m.PreId == id).ToList();
            var AllAnswerList = ServiceBase.FindBy<QuestionAndAnswer>(m => m.IsQuestion == false);
            List<QuestionViewModel> AnswerList = new List<QuestionViewModel>();
            foreach (var item in list)
            {
                QuestionViewModel answer = new QuestionViewModel();
                //回答
                answer.Question = item;
                //回答的回复
                answer.Answer = AllAnswerList.Where(m => m.PreId == item.Id).ToList();
                //回复的数量
                answer.AnswerCount = answer.Answer.Count();
                AnswerList.Add(answer);
            }
            return Response(new { question = question, answer = AnswerList, answerCount = list.Count() });
        }

        /// <summary>
        /// 获取回复列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAnswerReply(string id)
        {
            var replyList = ServiceBase.FindBy<QuestionAndAnswer>(m => m.PreId == id).ToList();
            return Response(replyList);
        }

        /// <summary>
        /// 提出或回答问题
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
