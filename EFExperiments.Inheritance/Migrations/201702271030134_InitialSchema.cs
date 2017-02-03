namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inheritance.root",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inheritance.intermediate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IntermediateProperty = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.root", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.leaf1",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty1 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.intermediate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.leaf2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.intermediate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.leaf3",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty3 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.root", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inheritance.leaf3", "Id", "Inheritance.root");
            DropForeignKey("Inheritance.leaf2", "Id", "Inheritance.intermediate");
            DropForeignKey("Inheritance.leaf1", "Id", "Inheritance.intermediate");
            DropForeignKey("Inheritance.intermediate", "Id", "Inheritance.root");
            DropIndex("Inheritance.leaf3", new[] { "Id" });
            DropIndex("Inheritance.leaf2", new[] { "Id" });
            DropIndex("Inheritance.leaf1", new[] { "Id" });
            DropIndex("Inheritance.intermediate", new[] { "Id" });
            DropTable("Inheritance.leaf3");
            DropTable("Inheritance.leaf2");
            DropTable("Inheritance.leaf1");
            DropTable("Inheritance.intermediate");
            DropTable("Inheritance.root");
        }
    }
}
