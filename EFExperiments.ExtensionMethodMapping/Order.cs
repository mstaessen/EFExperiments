using System;

namespace EFExperiments.ExtensionMethodMapping
{
    public class Order : AggregateRoot<Guid>
    {
        public string Client { get; set; }
    }
}