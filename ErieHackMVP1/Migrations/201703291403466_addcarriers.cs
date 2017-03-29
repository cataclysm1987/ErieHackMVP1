namespace ErieHackMVP1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcarriers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Carrier", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Carrier");
        }
    }
}
