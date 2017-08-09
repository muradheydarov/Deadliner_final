namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskToUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.TaskToUsers", new[] { "TaskModel_Id" });
            DropTable("dbo.TasksModels");
            DropTable("dbo.TaskToUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TaskToUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserIdInt = c.Int(nullable: false),
                        UserId = c.String(),
                        TaskId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        TaskModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TasksModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Heading = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TaskToUsers", "TaskModel_Id");
            CreateIndex("dbo.TaskToUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels", "Id");
            AddForeignKey("dbo.TaskToUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
