using System.Data.Entity;

namespace EFExperiments.ExtensionMethodMapping
{
    public class ExtensionMethodMappingContext : DbContext
    {
        internal const string SchemaName = "ExtensionMethodMapping";

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}