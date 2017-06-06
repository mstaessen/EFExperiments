using System;

namespace EFExperiments.Inheritance
{
    public abstract class TpcRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }
    }

    public abstract class TpcIntermediate : TpcRoot
    {
        public string IntermediateProperty { get; set; }
    }

    public class TpcLeafType1 : TpcIntermediate
    {
        public string LeafProperty1 { get; set; }
    }

    public class TpcLeafType2 : TpcIntermediate
    {
        public string LeafProperty2 { get; set; }
    }

    public class TpcLeafType3 : TpcRoot
    {
        public string LeafProperty3 { get; set; }
    }
}