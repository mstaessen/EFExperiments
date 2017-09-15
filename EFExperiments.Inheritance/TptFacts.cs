using System;
using System.Data.Entity;
using System.Linq;
using EFExperiments.Inheritance.Migrations;
using Xunit;

namespace EFExperiments.Inheritance
{
    public class TptFacts
    {
        public TptFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InheritanceContext, Configuration>());
        }

        [Fact]
        public void QueriesOnLeafTypesOnlyIncludeLeafTypeTableAndUp()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TptLeafType1>();
                var sql = query.ToString();
                Assert.Contains("tpt_root", sql);
                Assert.Contains("tpt_intermediate", sql);
                Assert.Contains("tpt_leaf1", sql);
                Assert.DoesNotContain("tpt_leaf2", sql);
                Assert.DoesNotContain("tpt_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnRootTypeIncludeAllTables()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TptRoot>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tpt_root", sql);
                Assert.Contains("tpt_intermediate", sql);
                Assert.Contains("tpt_leaf1", sql);
                Assert.Contains("tpt_leaf2", sql);
                Assert.Contains("tpt_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnIntermediateTypeIncludesChildTablesAndUp()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TptIntermediate>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tpt_root", sql);
                Assert.Contains("tpt_intermediate", sql);
                Assert.Contains("tpt_leaf1", sql);
                Assert.Contains("tpt_leaf2", sql);
                Assert.DoesNotContain("tpt_leaf3", sql);
            }
        }

        [Fact]
        public void QueriesOnAlternateIdentifier()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TptRoot>().Where(x => x.AlternateIdentifiers.Any(y => y.Identifier == "SomeIdentifier"));
                var sql = query.ToString();
                Assert.Contains("tpt_root", sql);
                Assert.Contains("tpt_intermediate", sql);
                Assert.Contains("tpt_leaf1", sql);
                Assert.Contains("tpt_leaf2", sql);
                Assert.Contains("tpt_leaf3", sql);
            }
        }
    }
}