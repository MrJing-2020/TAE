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
    using TAE.Data.Model.ViewModel;
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
        [HttpGet]
        public HttpResponseMessage DepUser(string id)
        {
            var list = new List<DepUserViewModel>();
            var dep = ServiceBase.FindBy<Department>(s => s.CompanyId == id).ToList();
            foreach (var item in dep)
            {
                DepUserViewModel depu = new DepUserViewModel();
                depu.Id = item.Id;
                depu.DepartName = item.DepartName;
                string sql = "select Id,CompanyId,DepartmentId,PositionId,RealName from AspNetUsers where DepartmentId = @DepID";
                SqlParameter parameter = new SqlParameter("@DepID", item.Id);
                depu.UserAuditViewModel = ServiceBase.FindBy<UserAuditViewModel>(sql,parameter).ToList();
                var workflow = ServiceBase.FindBy<WorkFlowDetail>(s => s.DepartmentId == item.Id).ToList();
                foreach (var k in workflow)
                {
                    if(k.DefualtAuditUserId==depu.UserAuditViewModel.FirstOrDefault().Id)
                    {
                        depu.UserAuditViewModel.FirstOrDefault().IsAudit = true;
                    }
                }
                list.Add(depu);
            }
            return Response(list);
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
            var wfid = ServiceBase.FindBy<WorkFlowDetail>(s => s.Step == flowDetail.Step && s.WorkFlowId==flowDetail.WorkFlowId).Select(s=>s.Id).ToArray();
            var list = ServiceBase.FindBy<WorkFlowDetail>(s => wfid.Contains(s.Id)).ToList();
            return Response(list);
        }

        [HttpPost]
        public HttpResponseMessage SubFlow(WorkFlow flow)
        {
            var flowEntity = ServiceBase.SaveEntity<WorkFlow>(flow);
            return Response(flowEntity);
        }

        [HttpPost]
        public HttpResponseMessage SubFlowDetail(List<WorkFlowDetail> flowDetail)
        {
            var WorkFid = flowDetail.FirstOrDefault().WorkFlowId;
            var list = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.WorkFlowId == WorkFid).ToList();
            if(list.Count==0)
            {
                foreach (var item in flowDetail)
                {
                    item.DepartmentId = ServiceIdentity.FindUser(s => s.Id == item.DefualtAuditUserId).FirstOrDefault().DepartmentId;
                    ServiceBase.SaveEntity<WorkFlowDetail>(item);
                }
            }
            else
            {
                var step = flowDetail.FirstOrDefault().Step;
                foreach (var item in list)
                {
                    if(item.Step>step)
                    {
                        var FirList = ServiceBase.FindBy<WorkFlowDetail>(s => s.Id == item.Id).FirstOrDefault();
                        FirList.Step = FirList.Step + 1;
                        ServiceBase.Update<WorkFlowDetail>(FirList);
                    }
                }
                foreach (var item in flowDetail)
                {
                    item.DepartmentId = ServiceIdentity.FindUser(s => s.Id == item.DefualtAuditUserId).FirstOrDefault().DepartmentId;
                    item.Step = item.Step + 1;
                    this.ServiceBase.SaveEntity<WorkFlowDetail>(item);
                }
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
        /// <summary>
        /// 步骤位置调整
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FlowDetailTop(string FlowId)
        {
            var list = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.Id == FlowId).FirstOrDefault();
            var Step = list.Step - 1;
            var listLast = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.Step == Step &&s.WorkFlowId==list.WorkFlowId).FirstOrDefault();
            if (list.Step != 1)
            {
                list.Step = list.Step - 1;
                listLast.Step = listLast.Step + 1;
                ServiceBase.Update<WorkFlowDetail>(list);
                ServiceBase.Update<WorkFlowDetail>(listLast);
            }
            return Response();
        }
        /// <summary>
        /// 步骤删除
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FlowDetailDel(string FlowId)
        {
            var WorkFlowId = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.Id == FlowId).FirstOrDefault();
            var list = ServiceBase.FindBy<WorkFlowDetail>().Where(s=>s.WorkFlowId==WorkFlowId.WorkFlowId && s.Step>WorkFlowId.Step).ToList();
            foreach (var item in list)
            {
                var listl = ServiceBase.FindBy<WorkFlowDetail>().Where(s => s.Id == item.Id).FirstOrDefault();
                listl.Step = listl.Step - 1;
                ServiceBase.Update<WorkFlowDetail>(listl);
            }
            ServiceBase.Remove<WorkFlowDetail>(FlowId);
            return Response();
        }

        /// <summary>
        /// 流程删除（包括流程步骤）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelFlow(OneParam model)
        {
            ServiceBase.Remove<WorkFlow>(m => m.Id == model.Id);
            ServiceBase.Remove<WorkFlowDetail>(m => m.WorkFlowId == model.Id);
            return Response(new { msg = "删除成功" });
        }
        
    }
}
