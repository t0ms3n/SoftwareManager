using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace SoftwareManager.DbMigrations.Migrations
{

    [Migration(2, TransactionBehavior.Default, "Create table Application")]
    public class CreateApplicationTable : Migration
    {
        private string _tableName = "Application";
        public override void Up()
        {
            Create.Table(_tableName).InSchema("dbo")
                .WithColumn($"{_tableName}Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("ApplicationIdentifier").AsGuid().NotNullable().Unique("IDX_Unique_ApplicationIdentifier")
                .WithColumn("CreateDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
                .WithColumn("CreateById").AsInt32().NotNullable()
                .WithColumn("ModifyDate").AsDateTime().Nullable()
                .WithColumn("ModifyById").AsInt32().Nullable()
                .WithColumn("RowVersion").AsCustom("rowversion").NotNullable();

            this.CreateForeignKeyWithIndex(_tableName, "CreateById", "ApplicationManager", "ApplicationManagerId");
            this.CreateForeignKeyWithIndex(_tableName, "ModifyById", "ApplicationManager", "ApplicationManagerId");
        }

        public override void Down()
        {
            Delete.Table(_tableName);
        }
    }

}