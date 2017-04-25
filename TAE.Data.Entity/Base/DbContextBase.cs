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
    public class DbContextBase : DbContextExtend
    {
        private static string connString = CachedConfigContext.Current.DaoConfig.TAE_Base;
        public DbContextBase()
            : base(connString)
        {
            //this.Database.Connection.ConnectionString = connString;
        }

        #region 映射的数据库表
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuRole> MenuRole { get; set; }
        public DbSet<DataRole> DataRole { get; set; }
        public DbSet<FilesInfo> FilesInfo { get; set; }
        public DbSet<WorkFlow> WorkFlow { get; set; }
        public DbSet<WorkFlowDetail> WorkFlowDetail { get; set; }
        public DbSet<WorkFlowDetailInfo> WorkFlowDetailInfo { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<TeachingPlan> TeachingPlan { get; set; }
        public DbSet<ClassInfo> ClassInfo { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseCategory> CourseCategory { get; set; }
        public DbSet<CourseSection> CourseSection { get; set; }
        public DbSet<TeacherInfo> TeacherInfo { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<PowerPoint> PowerPoint { get; set; }
        public DbSet<Handouts> Handouts { get; set; }
        //public DbSet<TestQuestion> TestQuestion { get; set; }
        //public DbSet<TestPaper> TestPaper { get; set; }
        //public DbSet<Examination> Examination { get; set; }
        public DbSet<QuestionAndAnswer> QuestionAndAnswer { get; set; }
        public DbSet<InviteAnswer> InviteAnswer { get; set; }

        public DbSet<UserRCourse> UserRCourse { get; set; }
        public DbSet<Notification> Notification { get; set; }

        #endregion
    }
}
