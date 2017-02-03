using System.Data.Entity;
using System.Threading.Tasks;
using EFExperiments.PrivateMembers.Migrations;
using Xunit;

namespace EFExperiments.PrivateMembers
{
    public class PrivateMemberFacts
    {
        public PrivateMemberFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PrivateMembersContext, Configuration>());
        }

        [Fact]
        public async Task EFCanPersistAndRetrieveEntitiesWithPrivateProperties()
        {
            IntrovertEntity insertedEntity;
            using (var context = new PrivateMembersContext()) {
                insertedEntity = new IntrovertEntity("Baby Doe");
                context.Introverts.Add(insertedEntity);
                await context.SaveChangesAsync();
            }

            using (var context = new PrivateMembersContext()) {
                var retrievedEntity = await context.Introverts.FindAsync(insertedEntity.Id);
                Assert.NotNull(retrievedEntity);
                Assert.Equal(insertedEntity.Id, retrievedEntity.Id);
                Assert.Equal(insertedEntity.Name, retrievedEntity.Name);
            }
        }
    }
}