using System;
using System.Collections.Generic;

namespace EFExperiments.Transactions
{
    public class TransactionalParentEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<TransactionalChildEntity> Children { get; set; }
    }
}