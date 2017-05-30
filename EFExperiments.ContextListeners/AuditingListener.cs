using System;
using System.Data.Entity;

namespace EFExperiments.ContextListeners
{
    public class AuditingListener : IDbContextListener
    {
        public void OnBeforeSaveChanges(DbContext context)
        {
            var date = DateTime.UtcNow;
            foreach (var entry in context.ChangeTracker.Entries<IAudited>()) {
                if (entry.State == EntityState.Added) {
                    entry.Entity.RowCreationDate = date;
                }
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted) {
                    entry.Entity.RowModificationDate = date;
                }
            }
        }
    }
}