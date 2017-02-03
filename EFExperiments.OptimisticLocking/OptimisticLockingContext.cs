using System;
using System.Data.Entity;

namespace EFExperiments.OptimisticLocking
{
    public class OptimisticLockingContext : DbContext
    {
        internal const string SchemaName = "OptimisticLocking";

        public IDbSet<VersionedEntity> VersionedEntities { get; set; }

        public IDbSet<AuditedEntity> AuditedEntities { get; set; }

        public IDbSet<ConcurrencyCheckedEntity> ConcurrencyCheckedEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditedEntity>()) {
                entry.Entity.LastUpdate = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }
    }
}