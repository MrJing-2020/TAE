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

    /// <summary>
    /// 基础服务类（提供基本增删查改，调用仓储层方法）
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        IRepositoryBase repositoryBase;
        public ServiceBase()
        {
            repositoryBase = ServiceHelper.GetService<IRepositoryBase>();
        }
        public ServiceBase(IRepositoryBase RepositoryBase)
        {
            repositoryBase = RepositoryBase;
        }

        #region 查询相关
        public T FindObject<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            return repositoryBase.FindObject<T>(conditions);
        }
        public T FindObject<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return repositoryBase.FindObject<T>(sql, parameters);
        }
        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            return repositoryBase.FindBy<T>(conditions);
        }
        public IQueryable<T> FindBy<T>() where T : class
        {
            return repositoryBase.FindBy<T>();
        }
        public IEnumerable<T> FindBy<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return repositoryBase.FindBy<T>(sql, parameters);
        }
        public IQueryable<T> FindAllByPage<T>(int pageNumber, int pageSize, out int total) where T : BaseModel
        {
            return repositoryBase.FindAllByPage<T>(pageNumber, pageSize,out total);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            return repositoryBase.FindAllByPage<T, TKey>(pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, bool isAsc = true) where T : BaseModel
        {
            return repositoryBase.FindAllByPage<T,object>(where, pageNumber, pageSize, out total, m=>m.Id, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            return repositoryBase.FindAllByPage<T, TKey>(where, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(string sql, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true, params SqlParameter[] parameters) where T : class
        {
            return repositoryBase.FindAllByPage<T, TKey>(sql, pageNumber, pageSize, out total, orderBy, isAsc, parameters);
        }
        public IEnumerable<T> FindAllByProc<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return repositoryBase.FindAllByProc<T>(sql, parameters);
        }
        public PageList<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, RequestArg arg) where T : class
        {
            int total;
            PageList<T> pageList = new PageList<T>() 
            {
                PageNumber=arg.PageNumber,
                PageSize=arg.PageSize,
            };
            pageList.DataList = repositoryBase.FindAllByPage<T, TKey>(where, arg.PageNumber, arg.PageSize, out total, orderBy, arg.IsAsc);
            pageList.Total = total;
            return pageList;
        }
        public PageList<T> FindAllByPage<T>(Expression<Func<T, bool>> where, RequestArg arg) where T : BaseModel
        {
            int total;
            PageList<T> pageList = new PageList<T>()
            {
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize
            };
            pageList.DataList = repositoryBase.FindAllByPage<T, object>(where, arg.PageNumber, arg.PageSize, out total, m => m.Id, arg.IsAsc);
            pageList.Total = total;
            return pageList;
        }
        public PageList<T> FindAllByPage<T>(RequestArg arg) where T : BaseModel
        {
            int total;
            PageList<T> pageList = new PageList<T>()
            {
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize,
            };
            pageList.DataList = repositoryBase.FindAllByPage<T>(arg.PageNumber, arg.PageSize, out total);
            pageList.Total = total;
            return pageList;
        }
        public PageList<T> FindAllByPage<T>(string sql, RequestArg arg, params SqlParameter[] parameters) where T : class
        {
            PageList<T> pageList = new PageList<T>()
            {
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize,
            };
            var listAll = repositoryBase.FindBy<T>(sql, parameters);
            pageList.Total = listAll.Count();
            pageList.DataList = listAll.Skip((arg.PageNumber - 1) * arg.PageSize).Take(arg.PageSize).AsQueryable<T>();
            return pageList;
        }

        #endregion

        #region 更新相关
        public T Update<T>(T entity) where T : class
        {
            return repositoryBase.Update<T>(entity);
        }
        public void Update<T>(params T[] entitis) where T : class
        {
            repositoryBase.Update<T>(entitis);
        }
        public T SaveEntity<T>(T entity) where T : BaseModel
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return Insert(entity);
            }
            else
            {
                return Update(entity);
            }
        }
        #endregion

        #region 新增相关
        public T Insert<T>(T entity) where T : BaseModel
        {
            entity.Id = Guid.NewGuid().ToString("n");
            return repositoryBase.Insert<T>(entity);
        }
        public void Insert<T>(IEnumerable<T> entities) where T : BaseModel
        {
            foreach (var item in entities)
            {
                item.Id = Guid.NewGuid().ToString("n");
            }
            repositoryBase.Insert<T>(entities);
        }

        public T InsertGeneral<T>(T entity) where T : class
        {
            return repositoryBase.Insert<T>(entity);
        }
        public void InsertGeneral<T>(IEnumerable<T> entities) where T : class
        {
            repositoryBase.Insert<T>(entities);
        }
        #endregion

        #region 删除相关
        public void Remove<T>(params object[] ids) where T : BaseModel
        {
            repositoryBase.Remove<T>(ids);
        }

        public void Remove<T>(Expression<Func<T, bool>> where) where T : class
        {
            repositoryBase.Remove<T>(where);
        }
        #endregion

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

        public void Dispose()
        {
            if (repositoryBase != null)
            {
                repositoryBase.Dispose();
            }
        }
    }
}
