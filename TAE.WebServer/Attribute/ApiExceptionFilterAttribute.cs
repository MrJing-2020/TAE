using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Filters;

namespace TAE.WebServer.Attribute
{
    /// <summary>
    /// 自定义异常过滤器
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute 
    {
        /// <summary>
        /// 重写基类的异常处理方法
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //异常日志记录（正式项目用log4net记录异常日志
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "——" +
            //    actionExecutedContext.Exception.GetType().ToString() + "：" + actionExecutedContext.Exception.Message + "——堆栈信息：" +
            //    actionExecutedContext.Exception.StackTrace);

            //返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotImplemented);
                actionExecutedContext.Response.Content = new StringContent("请求未执行");
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.RequestTimeout);
                actionExecutedContext.Response.Content = new StringContent("请求超时");
            }
            //如果找不到相应的异常，统一返回服务端错误500
            else
            {
                //exploitHint未开发人员提示
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { msg = "服务器错误!", exploitHint = "开发人员提示:"+actionExecutedContext.Exception.Message });
                //actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                //actionExecutedContext.Response.Content = new StringContent("服务器错误");
            }
            base.OnException(actionExecutedContext);
        }
    }
}