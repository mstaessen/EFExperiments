using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EFExperiments.ExtensionMethodMapping
{
    public static class EntityTypeConfigurationExtensions
    {
        private const string DefaultKeyColumnName = "Id";
        private const string DefaultCreationDateColumnName = "RowCreationDate";
        private const string DefaultModificationDateColumnName = "RowModificationDate";
        private const string DefaultRowVersionColumnName = "RowVersion";

        public static void MapAggregateRootId<TAggregateRoot, TKey>(this EntityTypeConfiguration<TAggregateRoot> configuration, string columnName, DatabaseGeneratedOption idGeneratedOption)
            where TAggregateRoot : AggregateRoot<TKey>
            where TKey : struct
        {
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Property(x => x.Id).HasColumnName(columnName).HasDatabaseGeneratedOption(idGeneratedOption);
            configuration.HasKey(x => x.Id);
        }

        public static void MapAggregateRootId<TAggregateRoot>(this EntityTypeConfiguration<TAggregateRoot> configuration, string columnName, DatabaseGeneratedOption idGeneratedOption)
            where TAggregateRoot : AggregateRoot<string>
        {
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Property(x => x.Id).HasColumnName(columnName).HasDatabaseGeneratedOption(idGeneratedOption);
            configuration.HasKey(x => x.Id);
        }

        public static void MapAggregateRootAuditProperties<TAggregateRoot, TKey>(this EntityTypeConfiguration<TAggregateRoot> configuration)
            where TAggregateRoot : AggregateRoot<TKey>
        {
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Property(x => x.RowCreationDate).HasColumnName(DefaultCreationDateColumnName);
            configuration.Property(x => x.RowModificationDate).HasColumnName(DefaultModificationDateColumnName);
        }

        public static void MapAggregateRootRowVersion<TAggregateRoot, TKey>(this EntityTypeConfiguration<TAggregateRoot> configuration)
            where TAggregateRoot : AggregateRoot<TKey>
        {
            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            configuration.Property(x => x.RowVersion).HasColumnName(DefaultRowVersionColumnName).IsRowVersion();
        }

        public static void MapDefaultAggregateRootProperties<TAggregateRoot, TKey>(this EntityTypeConfiguration<TAggregateRoot> configuration, DatabaseGeneratedOption idGeneratedOption)
            where TAggregateRoot : AggregateRoot<TKey>
            where TKey : struct
        {
            configuration.MapAggregateRootId<TAggregateRoot, TKey>(DefaultKeyColumnName, idGeneratedOption);
            configuration.MapAggregateRootAuditProperties<TAggregateRoot, TKey>();
            configuration.MapAggregateRootRowVersion<TAggregateRoot, TKey>();
        }

        public static void MapDefaultAggregateRootProperties<TAggregateRoot>(this EntityTypeConfiguration<TAggregateRoot> configuration, DatabaseGeneratedOption idGeneratedOption)
            where TAggregateRoot : AggregateRoot<string>
        {
            configuration.MapAggregateRootId(DefaultKeyColumnName, idGeneratedOption);
            configuration.MapAggregateRootAuditProperties<TAggregateRoot, string>();
            configuration.MapAggregateRootRowVersion<TAggregateRoot, string>();
        }
    }
}