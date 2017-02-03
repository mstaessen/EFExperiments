using System.Data.Entity;

namespace EFExperiments.Aggregates
{
    public class AggregatesContext : DbContext
    {
        internal const string SchemaName = "Aggregates";

        public DbSet<AggregateRoot> Aggregate { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}
