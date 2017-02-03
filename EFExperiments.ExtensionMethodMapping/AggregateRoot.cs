using System;

namespace EFExperiments.ExtensionMethodMapping
{
    public abstract class AggregateRoot<TKey> : IEntity<TKey>, IAuditedEntity, IVersionedEntity
    {
        public TKey Id { get; protected set; }

        public DateTime RowCreationDate { get; set; }

        public DateTime RowModificationDate { get; set; }

        public byte[] RowVersion { get; set; }
    }
}