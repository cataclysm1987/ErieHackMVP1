namespace ErieHackMVP1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fulladdress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StreetAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "State", c => c.String());
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ZipCode");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "StreetAddress");
        }
    }
}
