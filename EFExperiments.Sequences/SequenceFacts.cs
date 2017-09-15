using Xunit;

namespace EFExperiments.Sequences
{
    public class SequenceFacts
    {
        public SequenceFacts()
        {
//            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SequenceContext, Configuration>());
        }

        [Fact]
        public void SaveChangesCreatesAnImplicitTransaction()
        {
            using (var context = new SequenceContext()) {
                
            }
        }
    }
}