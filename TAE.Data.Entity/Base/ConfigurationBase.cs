namespace TAE.Data.Entity
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    public sealed class ConfigurationBase : DbMigrationsConfiguration<DbContextBase>
    {
        public ConfigurationBase()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MArc.Entity.DbContextBase";
        }

        protected override void Seed(DbContextBase context)
        {
            //可在此处初始化数据
        }
    }
}

