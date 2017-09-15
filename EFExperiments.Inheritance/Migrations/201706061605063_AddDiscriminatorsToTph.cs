namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscriminatorsToTph : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inheritance.AlternateTphIdentifiers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Identifier = c.String(),
                        TphRoot_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inheritance.tph_root", t => t.TphRoot_Id)
                .Index(t => t.TphRoot_Id);
            
            AlterColumn("Inheritance.tph_root", "Discriminator", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("Inheritance.AlternateTphIdentifiers", "TphRoot_Id", "Inheritance.tph_root");
            DropIndex("Inheritance.AlternateTphIdentifiers", new[] { "TphRoot_Id" });
            AlterColumn("Inheritance.tph_root", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropTable("Inheritance.AlternateTphIdentifiers");
        }
    }
}
