namespace EFExperiments.Aggregates.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Aggregates.AggregateRoots",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Aggregates.ChildEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EmailAddress = c.String(),
                        AggregateRoot_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Aggregates.AggregateRoots", t => t.AggregateRoot_Id)
                .Index(t => t.AggregateRoot_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Aggregates.ChildEntities", "AggregateRoot_Id", "Aggregates.AggregateRoots");
            DropIndex("Aggregates.ChildEntities", new[] { "AggregateRoot_Id" });
            DropTable("Aggregates.ChildEntities");
            DropTable("Aggregates.AggregateRoots");
        }
    }
}
