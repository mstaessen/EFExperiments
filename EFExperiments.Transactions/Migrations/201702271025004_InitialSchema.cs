namespace EFExperiments.Transactions.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Transactions.TransactionalParentEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Transactions.TransactionalChildEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        TransactionalParentEntity_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Transactions.TransactionalParentEntities", t => t.TransactionalParentEntity_Id)
                .Index(t => t.TransactionalParentEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Transactions.TransactionalChildEntities", "TransactionalParentEntity_Id", "Transactions.TransactionalParentEntities");
            DropIndex("Transactions.TransactionalChildEntities", new[] { "TransactionalParentEntity_Id" });
            DropTable("Transactions.TransactionalChildEntities");
            DropTable("Transactions.TransactionalParentEntities");
        }
    }
}
