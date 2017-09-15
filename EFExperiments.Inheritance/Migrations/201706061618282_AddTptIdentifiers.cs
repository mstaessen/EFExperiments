namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTptIdentifiers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inheritance.AlternateTptIdentifiers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Identifier = c.String(),
                        TptRoot_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tpt_root", t => t.TptRoot_Id)
                .Index(t => t.TptRoot_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inheritance.AlternateTptIdentifiers", "TptRoot_Id", "Inheritance.tpt_root");
            DropIndex("Inheritance.AlternateTptIdentifiers", new[] { "TptRoot_Id" });
            DropTable("Inheritance.AlternateTptIdentifiers");
        }
    }
}
