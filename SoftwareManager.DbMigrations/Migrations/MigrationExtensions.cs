using System.Data;
using FluentMigrator;

namespace SoftwareManager.DbMigrations.Migrations
{
    public static class MigrationExtensions
    {
        public static void CreateForeignKeyWithIndex(this Migration migration, string fromTable, string fromColumn, string toTable, string toColumn, bool withCascadeDelete = false)
        {

            var mig = migration.Create.ForeignKey(GetForeignKeyName(fromTable, toTable, fromColumn))
                .FromTable(fromTable)
                .ForeignColumn(fromColumn)
                .ToTable(toTable)
                .PrimaryColumn(toColumn);
            if (withCascadeDelete)
                mig.OnDelete(Rule.Cascade);

            migration.CreateForeignKeyIndex(fromTable, fromColumn, toTable);
        }


        public static void CreateForeignKeyIndex(this Migration migration, string fromTable, string indexColumnName, string toTable)
        {
            migration.Create.Index(GetIndexName(fromTable, toTable, indexColumnName))
                .OnTable(fromTable)
                .OnColumn(indexColumnName)
                .Ascending()
                .WithOptions()
                .NonClustered();
        }

        public static void DeleteForeignKeyWithIndex(this Migration migration, string fromTable, string toTable, string fromColumn)
        {
            migration.Delete.ForeignKey(GetForeignKeyName(fromTable, toTable, fromColumn)).OnTable(fromTable);
            migration.Delete.Index(GetIndexName(fromTable, toTable, fromColumn)).OnTable(fromTable);
        }

        public static string GetForeignKeyName(string fromTable, string toTable, string fromColumn)
        {
            return $"FK_{fromTable}_{toTable}_{fromColumn}";
        }

        public static string GetIndexName(string fromTable, string toTable, string indexColumn)
        {
            return $"IDX_FK_{fromTable}_{toTable}_{indexColumn}";
        }
    }
}
