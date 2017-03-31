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
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute 
    {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //异常日志记录（正式项目用log4net记录异常日志
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "——" +
            //    actionExecutedContext.Exception.GetType().ToString() + "：" + actionExecutedContext.Exception.Message + "——堆栈信息：" +
            //    actionExecutedContext.Exception.StackTrace);

            //返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotImplemented, new { error_description = "请求未执行!" });
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.RequestTimeout, new { error_description = "请求超时!" });
            }
            //如果找不到相应的异常，统一返回服务端错误500
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { error_description = "服务器错误!", exploitHint = actionExecutedContext.Exception.Message });
            }
            base.OnException(actionExecutedContext);
        }
    }
}