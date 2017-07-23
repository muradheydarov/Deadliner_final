namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskAssign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskAssigneds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskModelId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        TasksModel_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TasksModels", t => t.TasksModel_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.TasksModel_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TasksModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Heading = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        DeadlineTask = c.Int(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.String(),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskAssigneds", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaskAssigneds", "TasksModel_Id", "dbo.TasksModels");
            DropForeignKey("dbo.TasksModels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TasksModels", new[] { "User_Id" });
            DropIndex("dbo.TaskAssigneds", new[] { "User_Id" });
            DropIndex("dbo.TaskAssigneds", new[] { "TasksModel_Id" });
            DropTable("dbo.TasksModels");
            DropTable("dbo.TaskAssigneds");
        }
    }
}
