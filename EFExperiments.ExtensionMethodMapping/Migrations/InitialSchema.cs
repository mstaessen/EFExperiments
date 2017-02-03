using System.Data.Entity.Migrations;

namespace EFExperiments.ExtensionMethodMapping.Migrations
{
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ExtensionMethodMapping.Order",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Client = c.String(),
                        RowCreationDate = c.DateTime(nullable: false),
                        RowModificationDate = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ExtensionMethodMapping.Product",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowCreationDate = c.DateTime(nullable: false),
                        RowModificationDate = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("ExtensionMethodMapping.Product");
            DropTable("ExtensionMethodMapping.Order");
        }
    }
}
