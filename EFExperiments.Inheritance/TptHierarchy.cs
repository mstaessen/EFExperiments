using System;
using System.Collections.Generic;

namespace EFExperiments.Inheritance
{
    public class AlternateTptIdentifier
    {
        public long Id { get; set; }

        public string Identifier { get; set; }
    }

    public abstract class TptRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }

        public virtual ICollection<AlternateTptIdentifier> AlternateIdentifiers { get; set; }
    }

    public abstract class TptIntermediate : TptRoot
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