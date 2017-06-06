using System.Data.Entity;
using EFExperiments.Inheritance.Migrations;

namespace EFExperiments.Inheritance
{
    public class TphFacts
    {
        public TphFacts()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InheritanceContext, Configuration>());
        }
    }
}