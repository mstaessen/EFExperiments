using System;
using System.ComponentModel.DataAnnotations;

namespace EFExperiments.OptimisticLocking
{
    public class AuditedEntity : ITitled
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [ConcurrencyCheck]
        public DateTime LastUpdate { get; set; }
    }
}