using System;

namespace EFExperiments.Inheritance
{
    public abstract class TptRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }
    }

    public class TptIntermediate : TptRoot
    {
        public string IntermediateProperty { get; set; }
    }

    public class TptLeafType1 : TptIntermediate
    {
        public string LeafProperty1 { get; set; }
    }

    public class TptLeafType2 : TptIntermediate
    {
        public string LeafProperty2 { get; set; }
    }

    public class TptLeafType3 : TptRoot
    {
        public string LeafProperty3 { get; set; }
    }
}