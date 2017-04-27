using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TAE.Data.Model;
using TAE.IRepository;
using TAE.IService;

namespace TAE.Service
{
    /// <summary>
    /// 基础服务类（提供基本增删查改，调用仓储层方法）
    /// </summary>
    public class ServiceExtend : IServiceExtend
    {
        public IRepositoryExtend Repository { get; set; }

        public ServiceExtend(IRepositoryExtend repository) 
        {
            Repository = repository;
        }
        #region 查询相关
        public T FindObject<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            return Repository.FindObject<T>(conditions);
        }
        public T FindObject<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return Repository.FindObject<T>(sql, parameters);
        }
        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            return Repository.FindBy<T>(conditions);
        }
        public IQueryable<T> FindBy<T>() where T : class
        {
            return Repository.FindBy<T>();
        }
        public IEnumerable<T> FindBy<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return Repository.FindBy<T>(sql, parameters);
        }
        public IQueryable<T> FindAllByPage<T>(int pageNumber, int pageSize, out int total) where T : BaseModel
        {
            return Repository.FindAllByPage<T>(pageNumber, pageSize, out total);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            return Repository.FindAllByPage<T, TKey>(pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, bool isAsc = true) where T : BaseModel
        {
            return Repository.FindAllByPage<T, object>(where, pageNumber, pageSize, out total, m => m.Id, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true) where T : class
        {
            return Repository.FindAllByPage<T, TKey>(where, pageNumber, pageSize, out total, orderBy, isAsc);
        }
        public IQueryable<T> FindAllByPage<T, TKey>(string sql, int pageNumber, int pageSize, out int total, Expression<Func<T, TKey>> orderBy, bool isAsc = true, params SqlParameter[] parameters) where T : class
        {
            return Repository.FindAllByPage<T, TKey>(sql, pageNumber, pageSize, out total, orderBy, isAsc, parameters);
        }
        public IEnumerable<T> FindAllByProc<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            return Repository.FindAllByProc<T>(sql, parameters);
        }
        public PageList<T> FindAllByPage<T, TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, RequestArg arg) where T : class
        {
            int total;
            PageList<T> pageList = new PageList<T>()
            {
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize,
            };
            pageList.DataList = Repository.FindAllByPage<T, TKey>(where, arg.PageNumber, arg.PageSize, out total, orderBy, arg.IsAsc);
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
            pageList.DataList = Repository.FindAllByPage<T, object>(where, arg.PageNumber, arg.PageSize, out total, m => m.Id, arg.IsAsc);
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
            pageList.DataList = Repository.FindAllByPage<T>(arg.PageNumber, arg.PageSize, out total);
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
            var listAll = Repository.FindBy<T>(sql, parameters);
            pageList.Total = listAll.Count();
            pageList.DataList = listAll.Skip((arg.PageNumber - 1) * arg.PageSize).Take(arg.PageSize).AsQueryable<T>();
            return pageList;
        }

        #endregion

        #region 更新相关
        public T Update<T>(T entity) where T : class
        {
            return Repository.Update<T>(entity);
        }
        public void Update<T>(params T[] entitis) where T : class
        {
            Repository.Update<T>(entitis);
        }
        public void Update<T>(IEnumerable<T> entitis) where T : class
        {
            Repository.Update<T>(entitis);
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
            return Repository.Insert<T>(entity);
        }
        public void Insert<T>(IEnumerable<T> entities) where T : BaseModel
        {
            foreach (var item in entities)
            {
                item.Id = Guid.NewGuid().ToString("n");
            }
            Repository.Insert<T>(entities);
        }

        public T InsertGeneral<T>(T entity) where T : class
        {
            return Repository.Insert<T>(entity);
        }
        public void InsertGeneral<T>(IEnumerable<T> entities) where T : class
        {
            Repository.Insert<T>(entities);
        }
        #endregion

        #region 删除相关
        public void Remove<T>(params object[] ids) where T : BaseModel
        {
            Repository.Remove<T>(ids);
        }

        public void Remove<T>(Expression<Func<T, bool>> where) where T : class
        {
            Repository.Remove<T>(where);
        }
        #endregion

        public void Dispose()
        {
            if (Repository != null)
            {
                Repository.Dispose();
            }
        }
    }
}
