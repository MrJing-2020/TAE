using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Service
{
    using TAE.IRepository;
    using TAE.IService;
    using TAE.Data.Model;
    using TAE.Core.ServiceProvider;

    public class ServiceBase : ServiceExtend, IServiceBase
    {
        public static IRepositoryBase repositoryBase = ServiceHelper.GetService<IRepositoryBase>();
        public ServiceBase()
            : base(repositoryBase)
        {
        }

        #region 审核相关
        /// <summary>
        /// 报审通用方法
        /// </summary>
        /// <param name="workFlowId">流程Id</param>
        /// <param name="linkId">待审项Id</param>
        /// <param name="bussType">业务类型</param>
        /// <param name="userIds">审核人(认为一人对应一个WorkFlowDetail)</param>
        /// <returns></returns>
        public bool AuditReport(string workFlowId, string linkId, int bussType, string[] userIds)
        {
            try
            {
                var flowDetail = FindBy<WorkFlowDetail>(m => m.WorkFlowId == workFlowId);
                List<WorkFlowDetailInfo> flowInfoList = new List<WorkFlowDetailInfo>();
                int i = 0;
                foreach (var item in flowDetail)
                {
                    WorkFlowDetailInfo info = new WorkFlowDetailInfo()
                    {
                        WorkFlowDetailId = item.Id,
                        LinkId = linkId,
                        BussType = bussType,
                        Status = 0,
                        UserId = userIds[i]
                    };
                    flowInfoList.Add(info);
                    i++;
                }
                Insert<WorkFlowDetailInfo>(flowInfoList);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 报审通用方法
        /// </summary>
        /// <param name="workFlowId">流程Id</param>
        /// <param name="linkId">待审项Id</param>
        /// <param name="bussType">业务类型</param>
        /// <param name="userIds">审核人(BindOptionModel中Id对应WorkFlowDetailId，BindIds数组对应用户Id，可为多用户)</param>
        /// <returns></returns>
        public bool AuditReport(string workFlowId, string linkId, int bussType, List<BindOptionModel> BindOption)
        {
            try
            {
                var flowDetail = FindBy<WorkFlowDetail>(m => m.WorkFlowId == workFlowId);
                List<WorkFlowDetailInfo> flowInfoList = new List<WorkFlowDetailInfo>();
                int i = 0;
                foreach (var item in flowDetail)
                {
                    WorkFlowDetailInfo info = new WorkFlowDetailInfo()
                    {
                        WorkFlowDetailId = item.Id,
                        LinkId = linkId,
                        BussType = bussType,
                        Status = 0,
                        UserId = string.Join("','", BindOption.Where(m => m.Id == item.Id.ToString()).FirstOrDefault().BindIds)
                    };
                    flowInfoList.Add(info);
                    i++;
                }
                Insert<WorkFlowDetailInfo>(flowInfoList);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="WorkFlowDetailInfoId"></param>
        /// <param name="linkId">待审项Id</param>
        /// <param name="idea">审核意见</param>
        /// <param name="pass">是否通过</param>
        /// <param name="isEnd">流程是否结束</param>
        /// <returns></returns>
        public bool Audit(string WorkFlowDetailInfoId, string linkId, string idea, bool pass, out bool isEnd)
        {
            try
            {
                var flowInfo = FindBy<WorkFlowDetailInfo>(m => m.WorkFlowDetailId == WorkFlowDetailInfoId).FirstOrDefault();
                flowInfo.Idea = idea;
                flowInfo.Status = pass == true ? 1 : 2;
                flowInfo.AuditTime = DateTime.Now;
                Update<WorkFlowDetailInfo>(flowInfo);
                if (pass == true)
                {
                    isEnd = FindBy<WorkFlowDetailInfo>(m => m.LinkId == linkId).Count() > 0 ? false : true;
                }
                else
                {
                    isEnd = true;
                }
            }
            catch (Exception)
            {
                isEnd = false;
                return false;
            }
            return true;
        } 
        #endregion

        //public void Dispose()
        //{
        //    if (repositoryBase != null)
        //    {
        //        repositoryBase.Dispose();
        //    }
        //}
    }
}
