namespace EFExperiments.ChangeTracking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ChangeTracking.Entities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("ChangeTracking.Entities");
        }
    }
}
