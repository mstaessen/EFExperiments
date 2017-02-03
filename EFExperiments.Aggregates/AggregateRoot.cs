using System;
using System.Collections.Generic;

namespace EFExperiments.Aggregates
{
    public class AggregateRoot
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<ChildEntity> ChildEntities { get; set; }
    }
}