namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDiscriminatorFromTph : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Inheritance.tph_root", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("Inheritance.tph_root", "Discriminator", c => c.String(maxLength: 128));
        }
    }
}
