using System.Data.Entity;

namespace EFExperiments.CustomMigrationHistoryTable
{
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
}