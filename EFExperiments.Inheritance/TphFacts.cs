using System;
using System.Data.Entity;
using System.Linq;
using EFExperiments.Inheritance.Migrations;
using Xunit;

namespace EFExperiments.Inheritance
{
    public class TphFacts
    {
        public TphFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InheritanceContext, Configuration>());
        }

        [Fact]
        public void QueriesOnLeafTypesOnlyIncludeLeafTypeTable()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TphLeafType1>();
                var sql = query.ToString();
                Assert.Contains("tph_root", sql);
                Assert.DoesNotContain("tph_leaf1", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf2", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf3", sql); // simply does not exist
            }
        }

        [Fact]
        public void QueriesOnRootTypeIncludeAllTables()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TphRoot>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tph_root", sql);
                Assert.DoesNotContain("tph_leaf1", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf2", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf3", sql); // simply does not exist
            }
        }

        [Fact]
        public void QueriesOnIntermediateTypeIncludesChildTablesAndUp()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TphIntermediate>().Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("tph_root", sql);
                Assert.DoesNotContain("tph_leaf1", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf2", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf3", sql); // simply does not exist
            }
        }

        [Fact]
        public void QueriesOnAlternateIdentifier()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<TphRoot>().Where(x => x.AlternateIdentifiers.Any(y => y.Identifier == "SomeIdentifier"));
                var sql = query.ToString();
                Assert.Contains("tph_root", sql);
                Assert.DoesNotContain("tph_leaf1", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf2", sql); // simply does not exist
                Assert.DoesNotContain("tph_leaf3", sql); // simply does not exist
            }
        }
    }
}