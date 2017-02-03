namespace EFExperiments.OptimisticLocking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "OptimisticLocking.AuditedEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "OptimisticLocking.ConcurrencyCheckedEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ConcurrencyToken = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "OptimisticLocking.VersionedEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("OptimisticLocking.VersionedEntities");
            DropTable("OptimisticLocking.ConcurrencyCheckedEntities");
            DropTable("OptimisticLocking.AuditedEntities");
        }
    }
}
