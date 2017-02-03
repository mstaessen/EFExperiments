using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EFExperiments.OptimisticLocking.Migrations;
using Xunit;

namespace EFExperiments.OptimisticLocking
{
    public class OptimisticLockingFacts
    {
        public OptimisticLockingFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OptimisticLockingContext, Configuration>());
        }

        [Fact]
        public void RowVersionsAreAutomaticallyUpdated()
        {
            var id = Guid.NewGuid();
            byte[] version;
            using (var ctx = new OptimisticLockingContext()) {
                var entity = new VersionedEntity {
                    Id = id,
                    Title = Guid.NewGuid().ToString()
                };
                ctx.VersionedEntities.Add(entity);
                ctx.SaveChanges();
                version = entity.RowVersion;
            }

            byte[] newVersion;
            using (var ctx = new OptimisticLockingContext()) {
                var entity = ctx.VersionedEntities.Find(id);
                Assert.Equal(version, entity.RowVersion);
                entity.Title = Guid.NewGuid().ToString();
                ctx.SaveChanges();
                newVersion = entity.RowVersion;
            }

            Assert.NotEqual(version, newVersion);
        }

        [Fact]
        public void ConcurrencyTokensAreNotAutomaticallyUpdated()
        {
            var id = Guid.NewGuid();
            byte[] version;
            using (var ctx = new OptimisticLockingContext()) {
                var entity = new ConcurrencyCheckedEntity {
                    Id = id,
                    Title = Guid.NewGuid().ToString()
                };
                ctx.ConcurrencyCheckedEntities.Add(entity);
                ctx.SaveChanges();
                version = entity.ConcurrencyToken;
            }

            byte[] newVersion;
            using (var ctx = new OptimisticLockingContext()) {
                var entity = ctx.ConcurrencyCheckedEntities.Find(id);
                Assert.Equal(version, entity.ConcurrencyToken);
                entity.Title = Guid.NewGuid().ToString();
                ctx.SaveChanges();
                newVersion = entity.ConcurrencyToken;
            }

            Assert.Equal(version, newVersion);
        }

        [Fact]
        public void UsingRowVersionWillNotResultInDirtyWrites()
        {
            var id = CreateOptimisticLockedEntity<VersionedEntity>();
            Assert.Throws<DbUpdateConcurrencyException>(() => UpdateEntityConcurrently<VersionedEntity>(id));
        }

        [Fact]
        public void UsingIncorrectlySetUpConcurrencyTokensWillResultInDirtyWrites()
        {
            var id = CreateOptimisticLockedEntity<ConcurrencyCheckedEntity>();
            UpdateEntityConcurrently<ConcurrencyCheckedEntity>(id);
        }

        [Fact]
        public void UsingCorrectlySetUpConcurrencyTokensWillNotResultInDirtyWrites()
        {
            var id = CreateOptimisticLockedEntity<AuditedEntity>();
            Assert.Throws<DbUpdateConcurrencyException>(() => UpdateEntityConcurrently<AuditedEntity>(id));
        }

        private static Guid CreateOptimisticLockedEntity<T>()
            where T : class, ITitled, new()
        {
            var id = Guid.NewGuid();
            using (var ctx = new OptimisticLockingContext()) {
                var entity = new T {
                    Id = id,
                    Title = Guid.NewGuid().ToString()
                };
                ctx.Set<T>().Add(entity);
                ctx.SaveChanges();
            }
            return id;
        }

        private static void UpdateEntityConcurrently<T>(Guid id)
            where T : class, ITitled, new()
        {
            using (var context = new OptimisticLockingContext())
            {
                var entity = context.Set<T>().Find(id);
                Assert.NotNull(entity);
                using (var concurrentContext = new OptimisticLockingContext())
                {
                    var concurrentEntity = concurrentContext.Set<T>().Find(id);
                    Assert.NotNull(concurrentEntity);
                    concurrentEntity.Title = Guid.NewGuid().ToString();
                    concurrentContext.SaveChanges();
                }
                entity.Title = Guid.NewGuid().ToString();
                context.SaveChanges();
            }
        }
    }
}