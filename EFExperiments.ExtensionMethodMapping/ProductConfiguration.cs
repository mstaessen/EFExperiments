using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EFExperiments.ExtensionMethodMapping
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product")
                .MapDefaultAggregateRootProperties<Product, Guid>(DatabaseGeneratedOption.None);

        }
    }
}