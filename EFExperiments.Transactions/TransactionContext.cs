using System.Data.Entity;

namespace EFExperiments.Transactions
{
    public class TransactionContext : DbContext
    {
        internal const string SchemaName = "Transactions";

        public DbSet<TransactionalParentEntity> TransactionalEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}