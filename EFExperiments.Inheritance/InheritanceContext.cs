using System.Data.Entity;

namespace EFExperiments.Inheritance
{
    public class InheritanceContext : DbContext
    {
        internal const string SchemaName = "Inheritance";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            // Table-per-hierarchy (TPH) -- default
            // hierarchy is merged into a single table. Best performance, but generally sparse table.
            modelBuilder.Entity<TphRoot>().ToTable("tph_root");
//            modelBuilder.Entity<TphLeafType1>().Map(m => m.Requires("Discriminator").HasValue("L1"));
//            modelBuilder.Entity<TphLeafType2>().Map(m => m.Requires("Discriminator").HasValue("L2"));
//            modelBuilder.Entity<TphLeafType3>().Map(m => m.Requires("Discriminator").HasValue("L3"));

            // Table-per-type (TPT)
            // every type in the hierarchy gets its own table. This could hurt performance.
            modelBuilder.Entity<TptRoot>().ToTable("tpt_root");
            modelBuilder.Entity<TptIntermediate>().ToTable("tpt_intermediate");
            modelBuilder.Entity<TptLeafType1>().ToTable("tpt_leaf1");
            modelBuilder.Entity<TptLeafType2>().ToTable("tpt_leaf2");
            modelBuilder.Entity<TptLeafType3>().ToTable("tpt_leaf3");

            // Table-per-class (table-per-concrete-type) (TPC)
            // 
            modelBuilder.Entity<TpcRoot>().HasMany(x => x.AlternateIdentifiers).WithRequired();
            modelBuilder.Entity<TpcIntermediate>();
            modelBuilder.Entity<TpcLeafType1>().Map(m => {
                m.MapInheritedProperties().ToTable("tpc_leaf1");
                m.Requires("Discriminator").HasValue("L1");
            });
            modelBuilder.Entity<TpcLeafType2>().Map(m => {
                m.MapInheritedProperties().ToTable("tpc_leaf2");
                m.Requires("Discriminator").HasValue("L2");
            });
            modelBuilder.Entity<TpcLeafType3>().Map(m => {
                m.MapInheritedProperties().ToTable("tpc_leaf3");
                m.Requires("Discriminator").HasValue("L3");
            });
        }
    }
}