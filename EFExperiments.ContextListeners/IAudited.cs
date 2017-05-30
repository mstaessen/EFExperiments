using System;

namespace EFExperiments.ContextListeners
{
    public interface IAudited
    {
        DateTime RowCreationDate { get; set; }

        DateTime RowModificationDate { get; set; }
    }
}