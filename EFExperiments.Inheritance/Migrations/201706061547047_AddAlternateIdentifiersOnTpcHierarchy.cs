namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlternateIdentifiersOnTpcHierarchy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inheritance.AlternateTpcIdentifiers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Identifier = c.String(),
                        TpcRoot_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Inheritance.AlternateTpcIdentifiers");
        }
    }
}
