namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inheritance.tph_root",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                        IntermediateProperty = c.String(),
                        LeafProperty1 = c.String(),
                        LeafProperty2 = c.String(),
                        LeafProperty3 = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inheritance.tpt_root",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inheritance.tpt_intermediate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IntermediateProperty = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tpt_root", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.tpt_leaf1",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty1 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tpt_intermediate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.tpt_leaf2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tpt_intermediate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.tpt_leaf3",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LeafProperty3 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tpt_root", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "Inheritance.tpc_leaf1",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                        IntermediateProperty = c.String(),
                        LeafProperty1 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inheritance.tpc_leaf2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                        IntermediateProperty = c.String(),
                        LeafProperty2 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inheritance.tpc_leaf3",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RootProperty = c.String(),
                        LeafProperty3 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inheritance.tpt_leaf3", "Id", "Inheritance.tpt_root");
            DropForeignKey("Inheritance.tpt_leaf2", "Id", "Inheritance.tpt_intermediate");
            DropForeignKey("Inheritance.tpt_leaf1", "Id", "Inheritance.tpt_intermediate");
            DropForeignKey("Inheritance.tpt_intermediate", "Id", "Inheritance.tpt_root");
            DropIndex("Inheritance.tpt_leaf3", new[] { "Id" });
            DropIndex("Inheritance.tpt_leaf2", new[] { "Id" });
            DropIndex("Inheritance.tpt_leaf1", new[] { "Id" });
            DropIndex("Inheritance.tpt_intermediate", new[] { "Id" });
            DropTable("Inheritance.tpc_leaf3");
            DropTable("Inheritance.tpc_leaf2");
            DropTable("Inheritance.tpc_leaf1");
            DropTable("Inheritance.tpt_leaf3");
            DropTable("Inheritance.tpt_leaf2");
            DropTable("Inheritance.tpt_leaf1");
            DropTable("Inheritance.tpt_intermediate");
            DropTable("Inheritance.tpt_root");
            DropTable("Inheritance.tph_root");
        }
    }
}
