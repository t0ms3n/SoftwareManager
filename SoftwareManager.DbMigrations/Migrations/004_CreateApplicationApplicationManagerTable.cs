using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace SoftwareManager.DbMigrations.Migrations
{

    [Migration(4, TransactionBehavior.Default, "Create table ApplicationApplicationManager")]
    public class CreateApplicationApplicationManagerTable : Migration
    {
        private string _tableName = "ApplicationApplicationManager";
        public override void Up()
        {
            Create.Table(_tableName).InSchema("dbo")
                .WithColumn($"{_tableName}Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ApplicationId").AsInt32().NotNullable()
                .WithColumn("ApplicationManagerId").AsInt32().NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
                .WithColumn("CreateById").AsInt32().NotNullable()
                .WithColumn("ModifyDate").AsDateTime().Nullable()
                .WithColumn("ModifyById").AsInt32().Nullable()
                .WithColumn("RowVersion").AsCustom("rowversion").NotNullable();

            this.CreateForeignKeyWithIndex(_tableName, "CreateById", "ApplicationManager", "ApplicationManagerId");
            this.CreateForeignKeyWithIndex(_tableName, "ModifyById", "ApplicationManager", "ApplicationManagerId");

            this.CreateForeignKeyWithIndex(_tableName, "ApplicationId", "Application", "ApplicationId", true);
            this.CreateForeignKeyWithIndex(_tableName, "ApplicationManagerId", "ApplicationManager", "ApplicationManagerId");
        }

        public override void Down()
        {
            Delete.Table(_tableName);
        }
    }

}