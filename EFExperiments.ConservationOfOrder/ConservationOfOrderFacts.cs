using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EFExperiments.ConservationOfOrder
{
    public class ConservationOfOrderFacts : IDisposable
    {
        public ConservationOfOrderFacts()
        {
            using (var context = new ConservationOfOrderContext()) {
                context.Truncate<Entity>();
            }
        }

        [Fact]
        public async Task EntitiesAreSavedInTheSameOrderAsTheyWereAdded()
        {
            var insertOrder = 1;
            using (var context = new ConservationOfOrderContext()) {
                for (var i = 0; i < 10000; i++) {
                    context.Entities.Add(new Entity {
                        InsertOrder = insertOrder++
                    });
                }
                await context.SaveChangesAsync();

                var entitiesThatWereAddedOutOfOrder = context.Entities.Any(x => x.Id != x.InsertOrder);
                Assert.False(entitiesThatWereAddedOutOfOrder);
            }
        }

        public void Dispose()
        {
            using (var context = new ConservationOfOrderContext()) {
                context.Truncate<Entity>();
            }
        }
    }
}