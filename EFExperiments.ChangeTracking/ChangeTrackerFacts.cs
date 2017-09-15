using System;
using System.Data.Entity;
using System.Linq;
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
        public void WhenAddingAnEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                Assert.Equal(EntityState.Added, context.ChangeTracker.Entries<Order>().Single().State);
            }
        }

        [Fact]
        public async Task WhenUpdatingAnEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.UpdateShippingAddress("Microsoft, 1 Microsoft Way, Redmond");
                Assert.Equal(EntityState.Modified, context.ChangeTracker.Entries<Order>().Single().State);
            }
        }

        [Fact]
        public async Task WhenUpdatingAnEntityWithIdenticalData()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.UpdateShippingAddress("Apple, 1 Infinite Loop, Cupertino");
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
            }
        }

        public async Task WhenDeletingAnEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.Include(x => x.Lines).SingleAsync(x => x.Id == order.Id);
                Assert.NotNull(fetchedOrder);
                context.Orders.Remove(fetchedOrder);
                Assert.Equal(EntityState.Deleted, context.ChangeTracker.Entries<Order>().Single().State);
            }
        }

        [Fact]
        public async Task WhenAddingAChildEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            var product = new Product("MacBook Pro 13", 1749.00m);
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.AddLine(product, 1);
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
                Assert.Equal(EntityState.Added, context.ChangeTracker.Entries<OrderLine>().Single().State);
            }
        }

        [Fact]
        public async Task WhenUpdatingAChildEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            var product = new Product("MacBook Pro 13", 1749.00m);
            order.AddLine(product, 1);
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.Lines.First().Amount = 10;
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
                Assert.Equal(EntityState.Modified, context.ChangeTracker.Entries<OrderLine>().Single().State);
                // this is weird: LinePrice is not updated in ChangeTracker, however, value is successfully persisted to database afterwards.
                Assert.Equal(product.Price, context.ChangeTracker.Entries<OrderLine>().Single().CurrentValues[nameof(OrderLine.LinePrice)]);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                Assert.Equal(product.Price * 10, fetchedOrder.Lines.First().LinePrice);
            }
        }

        [Fact]
        public async Task WhenUpdatingAChildEntityWithIdenticalData()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            var product = new Product("MacBook Pro 13", 1749.00m);
            order.AddLine(product, 1);
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.Lines.First().Amount = 1;
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<OrderLine>().Single().State);
            }
        }

        [Fact]
        public async Task WhenRemovingAChildEntity()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            var product = new Product("MacBook Pro 13", 1749.00m);
            order.AddLine(product, 1);
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.Include(x => x.Lines).FirstAsync(x => x.Id == order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.RemoveLine(order.Lines.First()); // this does not work because it bypasses the proxy!
//                fetchedOrder.Lines.Remove(fetchedOrder.Lines.First());
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
                Assert.Equal(EntityState.Deleted, context.ChangeTracker.Entries<OrderLine>().Single().State);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.Include(x => x.Lines).FirstAsync(x => x.Id == order.Id);
                Assert.NotNull(fetchedOrder);
                Assert.Empty(fetchedOrder.Lines);

                // OrderLine is deleted because OrderId is part of the Primary Key
                var fetchedOrderLine = await context.Set<OrderLine>().FirstOrDefaultAsync(x => x.OrderId == order.Id);
                Assert.Null(fetchedOrderLine);
            }
        }

        [Fact]
        public async Task WhenRemovingAChildEntity2()
        {
            var order = new Order("Apple, 1 Infinite Loop, Cupertino");
            var product = new Product("MacBook Pro 13", 1749.00m);
            order.AddLine2(product, 1);
            using (var context = new ChangeTrackingContext()) {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
            using (var context = new ChangeTrackingContext()) {
                var fetchedOrder = await context.Orders.Include(x => x.Lines2).FirstAsync(x => x.Id == order.Id);
                Assert.NotNull(fetchedOrder);
                fetchedOrder.Lines2.Remove(fetchedOrder.Lines2.First());
                Assert.Equal(EntityState.Unchanged, context.ChangeTracker.Entries<Order>().Single().State);
                Assert.Equal(EntityState.Modified, context.ChangeTracker.Entries<OrderLine2>().Single().State);
                // EF would like to set the FK to a null value but the property is not nullable.
                await Assert.ThrowsAsync<InvalidOperationException>(() => context.SaveChangesAsync());
            }
        }
    }
}