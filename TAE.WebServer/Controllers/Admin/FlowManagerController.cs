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
        public HttpResponseMessage GetAllFlow(dynamic param)
        {
            string sqlGetAll = "select * from WorkFlow";
            var list = GetPageList<FlowViewModel>(param, sqlGetAll);
            var listDetail = ServiceBase.FindBy<WorkFlowDetail>(m => m.IsDel == false);
            foreach (var item in list.DataList)
            {
                string tempId = item.Id;
                item.WorkFlowDetail = listDetail.Where(m => m.WorkFlowId == tempId).ToList();
            }
            if (list.Count()>0)
            {
                return Response(list);
            }
            else
            {
                return Response(HttpStatusCode.NotFound, "未找到任何信息");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetFlowDetails(string id)
        {
            var list = ServiceBase.FindBy<WorkFlowDetail>(m => m.WorkFlowId == id).ToList();
            if (list.Count() > 0)
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
