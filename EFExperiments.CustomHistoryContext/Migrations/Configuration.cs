using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace EFExperiments.CustomMigrationHistoryTable.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomMigrationHistoryTableContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetHistoryContextFactory("System.Data.SqlClient", (connection, defaultSchema) => new CustomHistoryContext(connection, defaultSchema));
        }

        protected override void Seed(CustomMigrationHistoryTableContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }

    internal class CustomHistoryContext : HistoryContext
    {
        public CustomHistoryContext(DbConnection existingConnection, string defaultSchema) 
            : base(existingConnection, defaultSchema) {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().ToTable("custom_migration_table");
        }
    }
}
