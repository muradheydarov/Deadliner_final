namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReplyToTasks",
                c => new
                    {
                        ReplyToTaskId = c.Int(nullable: false, identity: true),
                        UserAnswer = c.String(nullable: false),
                        AnswerTime = c.DateTime(),
                        TaskToUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReplyToTaskId)
                .ForeignKey("dbo.TaskToUsers", t => t.TaskToUserID, cascadeDelete: true)
                .Index(t => t.TaskToUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplyToTasks", "TaskToUserID", "dbo.TaskToUsers");
            DropIndex("dbo.ReplyToTasks", new[] { "TaskToUserID" });
            DropTable("dbo.ReplyToTasks");
        }
    }
}
