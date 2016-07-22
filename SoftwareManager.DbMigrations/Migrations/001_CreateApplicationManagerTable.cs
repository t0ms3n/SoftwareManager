using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace SoftwareManager.DbMigrations.Migrations
{
    [Migration(1, TransactionBehavior.Default, "Create table ApplicationManager")]
    public class CreateApplicationManagerTable : Migration
    {
        private string _tableName = "ApplicationManager";
        public override void Up()
        {
            Create.Table(_tableName).InSchema("dbo")
                .WithColumn($"{_tableName}Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("LoginName").AsString(100).NotNullable()
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsAdmin").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("RowVersion").AsCustom("rowversion").NotNullable();

            Create.Index("IDX_LoginName")
                .OnTable(_tableName)
                .OnColumn("LoginName")
                .Ascending()
                .WithOptions()
                .NonClustered();

            Insert.IntoTable(_tableName).Row(new { Name = "Demo Admin", LoginName = "domain.de\\AdminDemo", IsActive = true, IsAdmin = true });
        }

        public override void Down()
        {
            Delete.Table(_tableName);
        }
    }
}
