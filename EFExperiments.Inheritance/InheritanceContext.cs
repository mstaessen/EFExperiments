using System.Data.Entity;

namespace EFExperiments.Inheritance
{
    public class InheritanceContext : DbContext
    {
        internal const string SchemaName = "Inheritance";

        public DbSet<HierarchyRoot> HierarchyRoots { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<HierarchyRoot>().ToTable("root");
            modelBuilder.Entity<HierarchyIntermediate>().ToTable("intermediate");
            modelBuilder.Entity<LeafType1>().ToTable("leaf1");
            modelBuilder.Entity<LeafType2>().ToTable("leaf2");
            modelBuilder.Entity<LeafType3>().ToTable("leaf3");
        }
    }
}