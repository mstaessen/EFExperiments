using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace EFExperiments.ContextListeners
{
    public class MyDbContext : DbContext
    {
        private readonly IDbContextListener[] listeners;

        public MyDbContext(IDbContextListener[] listeners)
        {
            this.listeners = listeners ?? new IDbContextListener[0];
        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            foreach (var listener in listeners) {
                listener.OnBeforeSaveChanges(this);
            }
            return base.SaveChanges();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var listener in listeners) {
                listener.OnBeforeSaveChanges(this);
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}