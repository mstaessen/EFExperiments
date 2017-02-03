using System;
using System.ComponentModel.DataAnnotations;

namespace EFExperiments.OptimisticLocking
{
    public class VersionedEntity : ITitled
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}