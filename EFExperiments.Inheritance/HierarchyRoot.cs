using System;

namespace EFExperiments.Inheritance
{
    public abstract class HierarchyRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }
    }
}