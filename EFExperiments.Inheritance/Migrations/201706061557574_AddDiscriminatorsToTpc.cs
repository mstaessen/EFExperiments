namespace EFExperiments.Inheritance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscriminatorsToTpc : DbMigration
    {
        public override void Up()
        {
            AddColumn("Inheritance.tpc_leaf1", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("Inheritance.tpc_leaf2", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("Inheritance.tpc_leaf3", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("Inheritance.tpc_leaf3", "Discriminator");
            DropColumn("Inheritance.tpc_leaf2", "Discriminator");
            DropColumn("Inheritance.tpc_leaf1", "Discriminator");
        }
    }
}
