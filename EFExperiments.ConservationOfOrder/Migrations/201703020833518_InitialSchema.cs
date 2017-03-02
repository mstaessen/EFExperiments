namespace EFExperiments.ConservationOfOrder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ConservationOfOrder.Entities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InsertOrder = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("ConservationOfOrder.Entities");
        }
    }
}
