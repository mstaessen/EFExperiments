using System;
using System.Data.Entity;
using System.Threading.Tasks;
using EFExperiments.ChangeTracking.Migrations;
using Xunit;

namespace EFExperiments.ChangeTracking
{
    public class ChangeTrackerFacts
    {
        public ChangeTrackerFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChangeTrackingContext, Configuration>());
        }

        [Fact]
        public async Task WhenATrackedEntityIsTouchedItsStateChangedToModified()
        {
            var id = Guid.NewGuid();
            using (var context = new ChangeTrackingContext()) {
                context.Entities.Add(new Entity {
                    Id = id,
                    Name = "John Doe",
                    BirthDate = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ChangeTrackingContext()) {
                var entity = await context.Entities.FindAsync(id);
                Assert.NotNull(entity);
                Assert.Equal(EntityState.Unchanged, context.Entry(entity).State);
                entity.Name = "Jane Doe";
                Assert.Equal(EntityState.Modified, context.Entry(entity).State);
            }
        }

        [Fact]
        public async Task WhenATrackedEntityIsTouchedItsStateChangedToModifiedUnlessYouReassignTheSameValue()
        {
            var id = Guid.NewGuid();
            using (var context = new ChangeTrackingContext()) {
                context.Entities.Add(new Entity {
                    Id = id,
                    Name = "John Doe",
                    BirthDate = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ChangeTrackingContext()) {
                var entity = await context.Entities.FindAsync(id);
                Assert.NotNull(entity);
                Assert.Equal(EntityState.Unchanged, context.Entry(entity).State);
                var entityName = entity.Name;
                entity.Name = entityName;
                Assert.Equal(EntityState.Unchanged, context.Entry(entity).State);
            }
        }
    }
}