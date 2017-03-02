using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace EFExperiments.ConservationOfOrder
{
    public class ConservationOfOrderContext : DbContext
    {
        internal const string SchemaName = "ConservationOfOrder";

        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(SchemaName);

            modelBuilder.Entity<Entity>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public void Truncate<TEntity>()
            where TEntity : class
        {
            var tableInfo = GetTableInfo(typeof(TEntity), this);
            Database.ExecuteSqlCommand($"TRUNCATE TABLE [{tableInfo.SchemaName}].[{tableInfo.TableName}]");
        }

        /// <summary>
        /// https://romiller.com/2014/04/08/ef6-1-mapping-between-types-tables/
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TableInfo GetTableInfo(Type type, DbContext context)
        {
            var metadata = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection) metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            // Find the storage entity set (table) that the entity is mapped
            var table = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            return new TableInfo {
                TableName = (string) table.MetadataProperties["Table"].Value ?? table.Name,
                SchemaName = (string) table.MetadataProperties["Schema"].Value ?? table.Schema
            };
        }
    }

    public class TableInfo
    {
        public string TableName { get; set; }

        public string SchemaName { get; set; }
    }
}