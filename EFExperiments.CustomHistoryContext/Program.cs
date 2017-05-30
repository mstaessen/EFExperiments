using System.Data.Entity;
using EFExperiments.CustomMigrationHistoryTable.Migrations;

namespace EFExperiments.CustomMigrationHistoryTable
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CustomMigrationHistoryTableContext, Configuration>());
            using (var context = new CustomMigrationHistoryTableContext()) {
                context.Database.Initialize(true);
            }
        }
    }
}
