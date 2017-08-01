namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskToUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserIdInt = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TaskId = c.Int(nullable: false),
                        TaskModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskModels", t => t.TaskModel_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TaskModel_Id);
            
            AddColumn("dbo.AspNetUsers", "ApplicationUserId", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskToUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TaskModels");
            DropIndex("dbo.TaskToUsers", new[] { "TaskModel_Id" });
            DropIndex("dbo.TaskToUsers", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "ApplicationUserId");
            DropTable("dbo.TaskToUsers");
            DropTable("dbo.TaskModels");
        }
    }
}
