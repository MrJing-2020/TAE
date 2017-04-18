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

        [HttpGet]
        public HttpResponseMessage AllFlowAndType()
        {
            var flowList = ServiceBase.FindBy<WorkFlow>();
            int typeGroup = Convert.ToInt32(TypeEnum.WorkFlow);
            var typeList = ServiceBase.FindBy<Type>(m => m.TypeGroup == typeGroup);
            List<JsTreeModel> treeList = new List<JsTreeModel>();
            foreach (var item in typeList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.TypeName,
                    parent = "#",
                    icon = "fa fa-folder",
                    state = new TreeStateModel { opened = true }
                };
                treeList.Add(treeItem);
            }
            foreach (var item in flowList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.Name,
                    parent = item.Type,
                    icon = "fa fa-folder"
                };
                treeList.Add(treeItem);
            }
            return Response(treeList);
        }

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
            if (list.Count() > 0)
            {
                return Response(list);
            }
            else
            {
                return Response(HttpStatusCode.NotFound, new { msg = "未找到任何信息" });
            }
        }

        [HttpGet]
        public HttpResponseMessage GetFlow(string id)
        {
            var flow = ServiceBase.FindBy<WorkFlow>(m => m.Id == id).FirstOrDefault();
            return Response(flow);
        }

        [HttpGet]
        public HttpResponseMessage GetFlowAndDetails(string id)
        {
            string sqlGetFlow = @"select a.*,b.TypeName,c.CompanyName,d.DepartName from WorkFlow a 
                                        inner join Type b on a.Type=b.Id
                                        inner join Company c on a.CompanyId=c.Id
                                        inner join Department d on a.DepartmentId =d.Id
                                        where a.Id=@Id";
            string sqlGetFlowDetails = @"select a.*,b.RealName as 'DefualtAuditRealName' from WorkFlowDetail a 
                                        inner join AspNetUsers b on a.DefualtAuditUserId=b.Id 
                                        where a.WorkFlowId=@WorkFlowId order by a.Step";
            SqlParameter param = new SqlParameter("@Id", id);
            SqlParameter paramDetail = new SqlParameter("@WorkFlowId", id);
            FlowViewModel flowDetail = ServiceBase.FindBy<FlowViewModel>(sqlGetFlow, param).FirstOrDefault();
            var list = ServiceBase.FindBy<FlowDetailViewModel>(sqlGetFlowDetails, paramDetail).ToList();
            flowDetail.WorkFlowDetail = list;
            return Response(flowDetail);
        }

        [HttpGet]
        public HttpResponseMessage GetFlowDetail(string id)
        {
            var flowDetail = ServiceBase.FindBy<WorkFlowDetail>(m => m.Id == id).FirstOrDefault();
            return Response(flowDetail);
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
            var WorkFid = flowDetail.WorkFlowId;
            var list = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.WorkFlowId == WorkFid).ToList();
            if(list.Count==0)
            {
                ServiceBase.SaveEntity<WorkFlowDetail>(flowDetail);
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Step > flowDetail.Step)
                    {
                        var FirList = ServiceBase.FindBy<WorkFlowDetail>(s => s.Id == item.Id).FirstOrDefault();
                        FirList.Step = FirList.Step + 1;
                        ServiceBase.Update<WorkFlowDetail>(FirList);
                    }
                }
                flowDetail.Step = flowDetail.Step + 1;
                this.ServiceBase.SaveEntity<WorkFlowDetail>(flowDetail);
            }
            return Response();
        }

        /// <summary>
        /// 工作流排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubFlowDetailSort(List<KeyValueModel> list)
        {
            var flowDetailList = ServiceBase.FindBy<WorkFlowDetail>().ToList();
            WorkFlowDetail[] entities = new WorkFlowDetail[list.Count()];
            int i = 0;
            foreach (var item in list)
            {
                var flowDetailItem = flowDetailList.Where(m => m.Id == item.Key).FirstOrDefault();
                flowDetailItem.Step = Convert.ToInt32(item.Value);
                entities[i] = flowDetailItem;
            }
            ServiceBase.Update<WorkFlowDetail>(entities);
            return Response();
        }

        /// <summary>
        /// 工作流下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FlowSelectList()
        {
            string sql = "select Id as 'Key',Name as 'Value' from WorkFlow";
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql).ToList();
            return Response(list);
        }

        /// <summary>
        /// 工作流类型下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FlowTypeSelectList()
        {
            string sql = "select Id as 'Key',TypeName as 'Value' from Type where TypeGroup = @group";
            SqlParameter param = new SqlParameter("@group", Convert.ToInt32(TypeEnum.WorkFlow));
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql, param).ToList();
            return Response(list);
        }

    }
}
