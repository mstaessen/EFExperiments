using System;
using System.Collections.Generic;

namespace EFExperiments.Inheritance
{
    public class AlternateTphIdentifier
    {
        public long Id { get; set; }

        public string Identifier { get; set; }
    }

    public abstract class TphRoot
    {
        public Guid Id { get; protected set; }

        public string RootProperty { get; set; }

        public virtual ICollection<AlternateTphIdentifier> AlternateIdentifiers { get; set; }
    }

    public abstract class TphIntermediate : TphRoot
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