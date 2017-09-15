namespace EFExperiments.ChangeTracking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ChangeTracking.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ShippingAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ChangeTracking.OrderLines",
                c => new
                    {
                        OrderId = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Description = c.String(),
                        ProductId = c.Guid(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Int(nullable: false),
                        LinePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderId, t.LineNumber })
                .ForeignKey("ChangeTracking.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "ChangeTracking.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ChangeTracking.OrderLines", "OrderId", "ChangeTracking.Orders");
            DropIndex("ChangeTracking.OrderLines", new[] { "OrderId" });
            DropTable("ChangeTracking.Products");
            DropTable("ChangeTracking.OrderLines");
            DropTable("ChangeTracking.Orders");
        }
    }
}
