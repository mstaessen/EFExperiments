using System;
using System.Data.Entity;
using System.Linq;
using EFExperiments.Inheritance.Migrations;
using Xunit;

namespace EFExperiments.Inheritance
{
    public class TpcFacts
    {
        public TpcFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InheritanceContext, Configuration>());
        }

        [Fact]
        public void QueriesOnLeafTypesOnlyIncludeLeafTypeTable()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TpcLeafType1>();
                var sql = query.ToString();
                Assert.Contains("tpc_leaf1", sql);
                Assert.DoesNotContain("tpc_leaf2", sql);
                Assert.DoesNotContain("tpc_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnRootTypeIncludeAllTables()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TpcRoot>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tpc_leaf1", sql);
                Assert.Contains("tpc_leaf2", sql);
                Assert.Contains("tpc_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnIntermediateTypeIncludesChildTablesAndUp()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TpcIntermediate>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tpc_leaf1", sql);
                Assert.Contains("tpc_leaf2", sql);
                Assert.DoesNotContain("tpc_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnAlternateIdentifier()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TpcRoot>().Where(x => x.AlternateIdentifiers.Any(y => y.Identifier == "SomeIdentifier"));
                var sql = query.ToString();
                Assert.Contains("tpc_leaf1", sql);
                Assert.Contains("tpc_leaf2", sql);
                Assert.Contains("tpc_leaf3", sql);
            }
        }
    }
}