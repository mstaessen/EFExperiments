using System;
using System.ComponentModel.DataAnnotations;

namespace EFExperiments.OptimisticLocking
{
    public class ConcurrencyCheckedEntity : ITitled
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [ConcurrencyCheck]
        public byte[] ConcurrencyToken { get; set; }
    }
}