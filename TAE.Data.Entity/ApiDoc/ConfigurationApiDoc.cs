namespace TAE.Data.Entity
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    public sealed class ConfigurationApiDoc : DbMigrationsConfiguration<DbContextApiDoc>
    {
        public ConfigurationApiDoc()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MArc.Entity.DbContextApiDoc";
        }

        protected override void Seed(DbContextApiDoc context)
        {
            //可在此处初始化数据
        }
    }
}

