using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EFExperiments.ExtensionMethodMapping
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Order")
                .MapDefaultAggregateRootProperties<Order, Guid>(DatabaseGeneratedOption.None);

        }
    }
}