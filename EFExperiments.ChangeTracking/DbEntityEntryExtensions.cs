using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace EFExperiments.ChangeTracking {
    public static class DbEntityEntryExtensions
    {
        public static IEnumerable<Difference> Compare<T>(this DbEntityEntry<T> entry)
            where T : class
        {
            return entry.OriginalValues.PropertyNames
                .Select(propertyName => new Difference(propertyName, entry.OriginalValues[propertyName], entry.CurrentValues[propertyName]));
        }
    }
}