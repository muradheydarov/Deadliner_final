namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskToUsers", "StudentReply", c => c.String(nullable: false));
            AddColumn("dbo.TaskToUsers", "AnswerTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskToUsers", "AnswerTime");
            DropColumn("dbo.TaskToUsers", "StudentReply");
        }
    }
}
