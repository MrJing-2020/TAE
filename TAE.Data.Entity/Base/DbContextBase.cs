using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;


namespace TAE.Data.Entity
{
    using TAE.Data.Model;
    using TAE.Core.Config;

    /// <summary>
    /// EF数据上下文基类
    /// </summary>
    public class DbContextBase : DbContext
    {
        private static string connString = CachedConfigContext.Current.DaoConfig.TAE_Base;
        public DbContextBase(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }
        public DbContextBase()
            : base(connString)
        {
            this.Database.Connection.ConnectionString = connString;
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            //防止生产复数表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        #region 基础增删查改

        public IEnumerable<T> FindAllBySql<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            IEnumerable<T> result;
            if (parameters.Count() > 0)
            {
                result = this.Database.SqlQuery<T>(sql, parameters);
            }
            else
            {
                result = this.Database.SqlQuery<T>(sql);
            }
            return result;
        }

        public T FindBySql<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            T result;
            if (parameters.Count() > 0)
            {
                result = this.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
            }
            else
            {
                result = this.Database.SqlQuery<T>(sql).FirstOrDefault();
            }
            return result;
        }

        public IEnumerable<T> FindAllByProc<T>(string procName, params SqlParameter[] parameters) where T : class
        {
            string procStr = "exec " + procName;
            IEnumerable<T> result;
            if (parameters.Count() > 0)
            {
                int i = 0;
                foreach (var item in parameters)
                {
                    procStr += " " + item.ParameterName + ",";
                    i++;
                }
                result = this.Database.SqlQuery<T>(procStr.TrimEnd(','), parameters);
            }
            else
            {
                result = this.Database.SqlQuery<T>(procStr);
            }
            return result;
        }
        public T Update<T>(T entity) where T : class
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = EntityState.Modified;
            this.SaveChanges();
            return entity;
        }
        public void Update<T>(params T[] entitis) where T : class
        {
            var set = this.Set<T>();
            foreach (var item in entitis)
            {
                this.Entry<T>(item).State = EntityState.Modified;
            }
            this.SaveChanges();
        }

        public T Insert<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
            this.SaveChanges();
            return entity;
        }

        public void Insert<T>(IEnumerable<T> entities) where T : class
        {
            this.Set<T>().AddRange(entities);
            this.SaveChanges();
        }

        public void Remove<T>(Expression<Func<T, bool>> where) where T : class
        {
            this.Set<T>().Delete(where);
            this.SaveChanges();
        }

        public void Remove<T>(params object[] ids) where T : BaseModel
        {
            this.Set<T>().Delete(ids);
            this.SaveChanges();
        } 
        #endregion

        #region 映射的数据库表
        public DbSet<Test> Test { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuRole> MenuRole { get; set; }

        #endregion
    }
}
