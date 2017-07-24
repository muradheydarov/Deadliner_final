namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppMigrationChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUserId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AspNetUsers", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUserId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AspNetUsers", "ApplicationUserId");
        }
    }
}
