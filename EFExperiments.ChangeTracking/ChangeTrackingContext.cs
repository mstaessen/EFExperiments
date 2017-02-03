using System.Data.Entity;

namespace EFExperiments.ChangeTracking
{
    public class ChangeTrackingContext  : DbContext
    {
        internal const string SchemaName = "ChangeTracking";

        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}
