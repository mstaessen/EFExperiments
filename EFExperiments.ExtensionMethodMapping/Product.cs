using System;

namespace EFExperiments.ExtensionMethodMapping
{
    public class Product : AggregateRoot<Guid>
    {
        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}