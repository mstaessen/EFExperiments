using System;

namespace EFExperiments.Inheritance
{
    public abstract class TphRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }
    }

    public class TphIntermediate : TphRoot
    {
        public string IntermediateProperty { get; set; }
    }

    public class TphLeafType1 : TphIntermediate
    {
        public string LeafProperty1 { get; set; }
    }

    public class TphLeafType2 : TphIntermediate
    {
        public string LeafProperty2 { get; set; }
    }

    public class TphLeafType3 : TphRoot
    {
        public string LeafProperty3 { get; set; }
    }
}