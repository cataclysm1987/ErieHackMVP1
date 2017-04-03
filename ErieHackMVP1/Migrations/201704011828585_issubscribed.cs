namespace ErieHackMVP1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issubscribed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsSubscribedToUpdates", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsSubscribedToUpdates");
        }
    }
}
