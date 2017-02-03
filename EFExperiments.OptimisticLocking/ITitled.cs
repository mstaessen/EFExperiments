using System;

namespace EFExperiments.OptimisticLocking
{
    public interface ITitled
    {
        Guid Id { get; set; }

        string Title { get; set; }
    }
}