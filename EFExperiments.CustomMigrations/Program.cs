using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using EFExperiments.CustomMigrations.Migrations;
using FluentMigrator;

namespace EFExperiments.CustomMigrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var scaffolder = new MigrationScaffolder(new Configuration());
            var migration = scaffolder.Scaffold("migration1");
            
            
                
                
            var migrator = new DbMigrator(new Configuration());
            using (var context = new CustomMigrationsDbContext()) {
                
            }
        }
    }

//    [Migration(1)]
//    private class TestMigration : Migration
//    {
//        public override void Up()
//        {
//            Create.PrimaryKey().OnTable().Columns()
//            Create.Table("Something")
//                .WithColumn("COl").
//        }
//
//        public override void Down()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}
