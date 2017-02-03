using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using EFExperiments.Transactions.Migrations;
using Xunit;
using Xunit.Abstractions;

namespace EFExperiments.Transactions
{
    public class TransactionFacts
    {
        private readonly ITestOutputHelper output;

        public TransactionFacts(ITestOutputHelper output)
        {
            this.output = output;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TransactionContext, Configuration>());
        }

        [Fact]
        public void SaveChangesCreatesAnImplicitTransaction()
        {
            string log;
            using (var dbContext = new TransactionContext()) {
                dbContext.TransactionalEntities.AddRange(new[] {
                    GenerateParentEntity(),
                    GenerateParentEntity(),
                    GenerateParentEntity()
                });
                dbContext.WithLogger(ctx => ctx.SaveChanges(), out log);
            }
            output.WriteLine(log);
            Assert.Contains("started transaction", log, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("committed transaction", log, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void SaveChangesDoesNotCreateTransactionIfInTransactionScope()
        {
            string log;
            using (var scope = new TransactionScope()) {
                using (var dbContext = new TransactionContext()) {
                    dbContext.TransactionalEntities.AddRange(new[] {
                        GenerateParentEntity(),
                        GenerateParentEntity(),
                        GenerateParentEntity()
                    });
                    dbContext.WithLogger(ctx => ctx.SaveChanges(), out log);
                }
                scope.Complete();
            }
            output.WriteLine(log);
            Assert.DoesNotContain("started transaction", log, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("committed transaction", log, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void MultipleContextsParticipateInSurroundingTransactionScope()
        {
            string log1;
            string log2;
            var parent1 = GenerateParentEntity();
            var parent2 = GenerateParentEntity();
            using (new TransactionScope()) {
                using (var dbContext = new TransactionContext()) {
                    dbContext.TransactionalEntities.Add(parent1);
                    dbContext.WithLogger(ctx => ctx.SaveChanges(), out log1);
                }

                using (var dbContext = new TransactionContext()) {
                    dbContext.TransactionalEntities.Add(parent2);
                    dbContext.WithLogger(ctx => ctx.SaveChanges(), out log2);
                }

                // We don't commit the transaction and checking if both saves are rolled back.
            }
            output.WriteLine(log1);
            output.WriteLine(log2);

            using (var dbContext = new TransactionContext()) {
                var results = dbContext.TransactionalEntities.Where(x => x.Id == parent1.Id || x.Id == parent2.Id);
                Assert.Empty(results);
            }
        }

        private static TransactionalParentEntity GenerateParentEntity()
        {
            return new TransactionalParentEntity {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Children = new List<TransactionalChildEntity> {
                    GenerateChildEntity()
                }
            };
        }

        private static TransactionalChildEntity GenerateChildEntity()
        {
            return new TransactionalChildEntity {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString()
            };
        }

        [Fact]
        public void EFContextsCanParticipateInSystemTransactions() {}
    }
}