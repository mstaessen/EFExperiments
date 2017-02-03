using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFExperiments.Aggregates.Migrations;
using Xunit;

namespace EFExperiments.Aggregates
{
    public class AggregatesFacts
    {
        public AggregatesFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AggregatesContext, Configuration>());
        }

        [Fact]
        public async Task SavingTheRootWillAlsoSaveTheChildren()
        {
            var id = Guid.NewGuid();
            using (var context = new AggregatesContext()) {
                context.Aggregate.Add(new AggregateRoot {
                    Id = id,
                    Name = "John Doe",
                    ChildEntities = new HashSet<ChildEntity> {
                        new ChildEntity {
                            Id = Guid.NewGuid(),
                            EmailAddress = "john.doe@example.com"
                        }
                    }
                });
                await context.SaveChangesAsync();
            }

            using (var context = new AggregatesContext()) {
                var aggregate = await context.Aggregate.Include(x => x.ChildEntities).SingleAsync(x => x.Id == id);
                Assert.NotNull(aggregate);
                Assert.NotEmpty(aggregate.ChildEntities);
                Assert.Equal("john.doe@example.com", aggregate.ChildEntities.Select(x => x.EmailAddress).First());
                await context.SaveChangesAsync();
            }
        }
    }
}