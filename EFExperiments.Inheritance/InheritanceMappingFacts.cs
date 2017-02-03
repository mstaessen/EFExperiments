using System;
using System.Data.Entity;
using System.Linq;
using EFExperiments.Inheritance.Migrations;
using Xunit;

namespace EFExperiments.Inheritance
{
    public class InheritanceMappingFacts
    {
        public InheritanceMappingFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InheritanceContext, Configuration>());
        }

        [Fact]
        public void WhenQueryingForLeafsOnlyRelevantNodesAreIncluded()
        {
            using (var context = new InheritanceContext()) {
                var query = context.Set<LeafType1>();
                var sql = query.ToString();
                Assert.Contains("root", sql);
                Assert.Contains("intermediate", sql);
                Assert.Contains("leaf1", sql);
                Assert.DoesNotContain("leaf2", sql);
            }
        }

        [Fact]
        public void WhenPerformingWildcardQueriesAllTypesInHierarchyAreInQuery()
        {
            using (var context = new InheritanceContext()) {
                var query = context.HierarchyRoots.Where(x => x.Id == Guid.Empty);
                var sql = query.ToString();
                Assert.Contains("root", sql);
                Assert.Contains("intermediate", sql);
                Assert.Contains("leaf1", sql);
                Assert.Contains("leaf2", sql);
                Assert.Contains("leaf3", sql);
            }
        }
    }
}