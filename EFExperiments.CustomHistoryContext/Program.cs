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

    public class CustomMigrationHistoryTableContext : DbContext
    {
        internal const string SchemaName = "CustomMigrationHistoryTable";

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }

    public class Product
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
