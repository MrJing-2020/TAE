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
            return repositoryBase.SaveEntity(entity);
        }
        #endregion

        #region 新增相关
        public T Insert<T>(T entity) where T : class
        {
            return repositoryBase.Insert<T>(entity);
        }
        public void Insert<T>(IEnumerable<T> entities) where T : class
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

        public void Dispose()
        {
            if (repositoryBase != null)
            {
                repositoryBase.Dispose();
            }
        }
    }
}
