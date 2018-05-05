namespace TaskManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DateStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Position", c => c.String());
            AddColumn("dbo.Users", "Login", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Role");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Login");
            DropColumn("dbo.Users", "Position");
            DropColumn("dbo.Users", "DateStart");
        }
    }
}
