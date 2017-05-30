using System.Data.Entity;

namespace EFExperiments.Logging
{
    public class LoggingContext : DbContext
    {
        internal const string SchemaName = "Logging";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}
