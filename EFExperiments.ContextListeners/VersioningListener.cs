using System;
using System.Data.Entity;

namespace EFExperiments.ContextListeners
{
    public class VersioningListener : IDbContextListener
    {
        private static readonly Random Random = new Random();

        public void OnBeforeSaveChanges(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IVersioned>()) {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted) {
                    var buffer = new byte[8];
                    Random.NextBytes(buffer);
                    entry.Entity.RowVersion = buffer;
                }
            }
        }
    }
}