using System;

namespace EFExperiments.ExtensionMethodMapping
{
    public interface IAuditedEntity
    {
        DateTime RowCreationDate { get; set; }

        DateTime RowModificationDate { get; set; }
    }
}