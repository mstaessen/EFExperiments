using System.Data.Entity;

namespace EFExperiments.PrivateMembers
{
    public class PrivateMembersContext : DbContext
    {
        internal const string SchemaName = "PrivateMembers";

        public DbSet<IntrovertEntity> Introverts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}