using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;

namespace EFExperiments.Sequences
{
    public class SequenceContext : DbContext
    {
        internal const string SchemaName = "Sequences";

        public DbSet<SequentialEntity> TransactionalEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<SequentialEntity>()
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnAnnotation("defaultValueSql", new DefaultValue());
        }
    }

    public class SequentialEntity
    {
        public long Id { get; set; }

        public string Title { get; set; }
    }
}
