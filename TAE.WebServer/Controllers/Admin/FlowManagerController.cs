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
        #region 私有成员

        private string sqlGetFlow = @"select a.*,b.CompanyName,c.DepartName from WorkFlow a 
                                        inner join Company b on a.CompanyId=b.Id
                                        inner join Department c on a.DepartmentId=c.Id"; 

        #endregion

        [HttpPost]
        public HttpResponseMessage AllFlows(dynamic param)
        {
            string sqlGetAll = sqlGetFlow;
            return GetDataList<FlowViewModel>(param, sqlGetAll);
        }

        [HttpPost]
        public HttpResponseMessage AllFlowAndDetails(dynamic param)
        {
            string sqlGetAll = sqlGetFlow;
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
        public HttpResponseMessage GetFlow(string id)
        {
            var flow = ServiceBase.FindBy<WorkFlow>(m => m.Id == id).FirstOrDefault();
            if (flow!=null)
            {
                return Response(flow);
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

        /// <summary>
        /// 工作流类型下拉框
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FlowTypeSelectList(string id)
        {
            string sql = "select Id as 'Key',PositionName as 'Value' from Position where CompanyId = @Id";
            SqlParameter param = new SqlParameter("@Id", id);
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql, param).ToList();
            return Response(list);
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

        [HttpGet]
        public HttpResponseMessage FlowSelectList()
        {
            var flowList = ServiceBase.FindBy<WorkFlow>();
            return Response();
        }

        /// <summary>
        /// 工作流排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubFlowDetail(List<KeyValueModel> list)
        {
            var flowDetailList = ServiceBase.FindBy<WorkFlowDetail>().ToList();
            WorkFlowDetail[] entities = new WorkFlowDetail[list.Count()];
            int i=0;
            foreach (var item in list)
            {
                var flowDetailItem=flowDetailList.Where(m=>m.Id==item.Key).FirstOrDefault();
                flowDetailItem.Step=Convert.ToInt32(item.Value);
                entities[i] = flowDetailItem;
            }
            ServiceBase.Update<WorkFlowDetail>(entities);
            return Response();
        }

    }
}
