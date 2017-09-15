using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EFExperiments.ChangeTracking
{
    public class ChangeTrackingContext  : DbContext
    {
        internal const string SchemaName = "ChangeTracking";

        /// <summary>
        /// An entity with a 1:N relationship
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// A standalone entity
        /// </summary>
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<Order>(c => {
                c.HasKey(x => x.Id);
                c.HasMany(x => x.Lines).WithRequired().HasForeignKey(x => x.OrderId);
                c.HasMany(x => x.Lines2).WithRequired().HasForeignKey(x => x.OrderId);
            });

            modelBuilder.Entity<OrderLine>(c => {
                c.HasKey(x => new { x.OrderId, x.LineNumber });
            });
        }
    }

    public static class ModelBuilderExtensions
    {
        public static DbModelBuilder Entity<TEntity>(this DbModelBuilder modelBuilder, Action<EntityTypeConfiguration<TEntity>> configure)
            where TEntity : class
        {
            var configuration = modelBuilder.Entity<TEntity>();
            configure?.Invoke(configuration);
            return modelBuilder;
        }
    }
}
