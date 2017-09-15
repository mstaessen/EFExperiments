namespace EFExperiments.ChangeTracking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlternativeLineMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ChangeTracking.OrderLine2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Description = c.String(),
                        ProductId = c.Guid(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Int(nullable: false),
                        LinePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ChangeTracking.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ChangeTracking.OrderLine2", "OrderId", "ChangeTracking.Orders");
            DropIndex("ChangeTracking.OrderLine2", new[] { "OrderId" });
            DropTable("ChangeTracking.OrderLine2");
        }
    }
}
