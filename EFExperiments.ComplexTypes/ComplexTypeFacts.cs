using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Xunit;

namespace EFExperiments.ComplexTypes
{
    public class ComplexTypeFacts
    {
        [Fact]
        public void CannotUseComplexTypesAsPrimaryKey()
        {
            
        }
    }

    public class Context1 : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("ComplexTypes");

            modelBuilder.Entity<Product>()
                .HasMany(x => x.AlternateIds);
            modelBuilder.ComplexType<ProductId>();
        }
    }

    [ComplexType]
    public class ProductId
    {
        public string Type { get; set; }

        public long Number { get; set; }
    }

    [Table("Product")]
    public class Product
    {
        public Guid Id { get; set; }

        public virtual ICollection<ProductId> AlternateIds { get; set; }
    }
}
