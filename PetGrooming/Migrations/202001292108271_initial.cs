namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Species", "Breed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Species", "Breed");
        }
    }
}
