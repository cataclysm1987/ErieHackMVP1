namespace ErieHackMVP1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SMSroute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SMSRoute", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SMSRoute");
        }
    }
}
