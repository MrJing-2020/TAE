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
    using Core.Cache;

    /// <summary>
    /// EF增删查改封装类（仓储层）
    /// </summary>
    public class RepositoryBase : IRepositoryBase
    {
        private Func<DbContextBase> func;
        public DbContextBase Context {
            get {
                return CacheHelper.GetItem<DbContextBase>("context", func);
            }
        }
        public RepositoryBase(DbContextBase context)
        {
            func = new Func<DbContextBase>(() => { return context; });
        }
        public RepositoryBase()
        {
            func = new Func<DbContextBase>(() => { return new DbContextBase(); });
        }

        #region 查询相关
        public T FindObject<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            var set = Context.Set<T>();
            return set.FindBy<T>(conditions).FirstOrDefault();
        }
        public T FindObject<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return Context.FindBySql<T>(sql, parameters);
        }
        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            var set = Context.Set<T>();
            return set.FindBy<T>(conditions);
        }
        public IQueryable<T> FindBy<T>() where T : class
        {
            var set = Context.Set<T>();
            return set.FindAll<T>();
        }
        public IEnumerable<T> FindBy<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return Context.FindAllBySql<T>(sql, parameters);
        }
        public IQueryable<T> FindAllByPage<T>(int pageNumber, int pageSize, out int total) where T : BaseModel
        {
            var set = Context.Set<T>();
            return set.FindAll<T>().GetPage<T>(out total,pageNumber,pageSize);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            var set = Context.Set<T>();
            return set.FindAll<T, TKey>(pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            var set = Context.Set<T>();
            return set.FindBy<T, TKey>(where, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(string sql, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true, params SqlParameter[] parameters) where T : class
        {
            var set = Context.Set<T>();
            return (Context.FindAllBySql<T>(sql, parameters).AsQueryable<T>()).FindBy<T, TKey>(m => true, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IEnumerable<T> FindAllByProc<T>(string procName, params SqlParameter[] parameters) where T : class
        {
            return Context.FindAllByProc<T>(procName, parameters);
        }
        #endregion

        #region 更新相关
        public T Update<T>(T entity) where T : class
        {
            return Context.Update<T>(entity);
        }
        public void Update<T>(params T[] entitis) where T : class
        {
            Context.Update<T>(entitis);
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
            return Context.Insert<T>(entity);
        }
        public void Insert<T>(IEnumerable<T> entities) where T : class
        {
            Context.Insert<T>(entities);
        } 
        #endregion

        #region 删除相关
        public void Remove<T>(params object[] ids) where T : BaseModel
        {
            Context.Remove<T>(ids);
        }

        public void Remove<T>(Expression<Func<T, bool>> where) where T : class
        {
            Context.Remove<T>(where);
        } 
        #endregion

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
