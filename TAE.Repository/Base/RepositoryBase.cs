using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Repository
{
    using TAE.Data.Entity;
    using TAE.IRepository;
    using TAE.Data.Model;

    /// <summary>
    /// EF增删查改分装类（仓储层）
    /// </summary>
    public class RepositoryBase : IRepositoryBase
    {
        DbContextBase context;
        public RepositoryBase(DbContextBase DbContext)
        {
            context = DbContext;
        }
        public RepositoryBase()
        {
            context = new DbContextBase();
        }

        #region 查询相关
        public T FindObject<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            var set = context.Set<T>();
            return set.FindBy<T>(conditions).FirstOrDefault();
        }
        public T FindObject<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return context.FindBySql<T>(sql, parameters);
        }
        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            var set = context.Set<T>();
            return set.FindBy<T>(conditions);
        }
        public IQueryable<T> FindBy<T>() where T : class
        {
            var set = context.Set<T>();
            return set.FindAll<T>();
        }
        public IEnumerable<T> FindBy<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return context.FindAllBySql<T>(sql, parameters);
        }
        public IQueryable<T> FindAllByPage<T>(int pageNumber, int pageSize, out int total) where T : BaseModel
        {
            var set = context.Set<T>();
            return set.FindAll<T>().GetPage<T>(out total,pageNumber,pageSize);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            var set = context.Set<T>();
            return set.FindAll<T, TKey>(pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            var set = context.Set<T>();
            return set.FindBy<T, TKey>(where, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(string sql, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true, params SqlParameter[] parameters) where T : class
        {
            var set = context.Set<T>();
            return (context.FindAllBySql<T>(sql, parameters).AsQueryable<T>()).FindBy<T, TKey>(m => true, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IEnumerable<T> FindAllByProc<T>(string procName, params SqlParameter[] parameters) where T : class
        {
            return context.FindAllByProc<T>(procName, parameters);
        }
        #endregion

        #region 更新相关
        public T Update<T>(T entity) where T : class
        {
            return context.Update<T>(entity);
        }
        public void Update<T>(params T[] entitis) where T : class
        {
            context.Update<T>(entitis);
        }
        public T SaveEntity<T>(T entity) where T : BaseModel
        {
            if (!string.IsNullOrEmpty(entity.Id))
            {
                Update<T>(entity);
            }
            else
            {
                Insert<T>(entity);
            }
            return entity;
        }
        #endregion

        #region 新增相关
        public T Insert<T>(T entity) where T : class
        {
            return context.Insert<T>(entity);
        }
        public void Insert<T>(IEnumerable<T> entities) where T : class
        {
            context.Insert<T>(entities);
        } 
        #endregion

        #region 删除相关
        public void Remove<T>(params object[] ids) where T : BaseModel
        {
            context.Remove<T>(ids);
        }

        public void Remove<T>(Expression<Func<T, bool>> where) where T : class
        {
            context.Remove<T>(where);
        } 
        #endregion

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
