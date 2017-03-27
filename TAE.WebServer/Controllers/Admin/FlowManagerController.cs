using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace TAE.WebServer.Controllers.Admin
{
    using System.Data.SqlClient;
    using TAE.Data.Model;
    using TAE.WebServer.Common;
    public class FlowManagerController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllFlow(int pageNumber = 1, int pageSize = RequestArg.defualtPageSize, string orderName = "")
        {
            RequestArg arg = new RequestArg()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            string sqlGetAll = "select * WorkFlow";
            if (!string.IsNullOrEmpty(orderName))
            {
                SqlParameter para = new SqlParameter("@orderName", orderName);
                sqlGetAll += " order by @orderName";
            }
            var list = ServiceBase.FindAllByPage<FlowViewModel>(sqlGetAll, arg);
            var listDetail = ServiceBase.FindBy<WorkFlowDetail>(m => m.IsDel == false);
            foreach (var item in list.DataList)
            {
                item.WorkFlowDetail = listDetail.Where(m => m.WorkFlowId == item.Id).ToList();
            }
            if (list != null)
            {
                return Response(list);
            }
            else
            {
                return Response(HttpStatusCode.NotFound, "未找到任何信息");
            }
        }

        [HttpPost]
        public HttpResponseMessage SubFlow(WorkFlow flow)
        {
            var flowEntity = ServiceBase.SaveEntity<WorkFlow>(flow);
            return Response(flowEntity);
        }

        [HttpPost]
        public HttpResponseMessage SubFlowDetail(WorkFlowDetail flowDetail)
        {
            var flowDetailEntity = ServiceBase.SaveEntity<WorkFlowDetail>(flowDetail);
            return Response(flowDetailEntity);
        }

    }
}
