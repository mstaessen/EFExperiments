using System.Data.Entity;

namespace EFExperiments.CustomMigrations
{
    public class CustomMigrationsDbContext : DbContext
    {
        internal const string SchemaName = "CustomMigrations";



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<Customer>();
        }
    }

    public class Customer
    {
        public long Id { get; private set; }

        public string Name { get; private set; }

        protected Customer() { }

        public Customer(string name) : this()
        {
            Name = name;
        }

        public void Rename(string name)
        {
            Name = name;
        }
    }
}