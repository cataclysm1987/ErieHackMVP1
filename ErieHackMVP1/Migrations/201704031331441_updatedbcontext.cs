namespace ErieHackMVP1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedbcontext : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reports", "ReportName", c => c.String(nullable: false));
            AlterColumn("dbo.Reports", "ReportDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Reports", "ReportCounty", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "ReportCounty", c => c.String());
            AlterColumn("dbo.Reports", "ReportDescription", c => c.String());
            AlterColumn("dbo.Reports", "ReportName", c => c.String());
        }
    }
}
