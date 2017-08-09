namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskToUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.TaskToUsers", "UserId");
            RenameColumn(table: "dbo.TaskToUsers", name: "ApplicationUser_Id", newName: "UserId");
            RenameColumn(table: "dbo.TaskToUsers", name: "TaskModel_Id", newName: "TasksModelId");
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_TaskModel_Id", newName: "IX_TasksModelId");
            DropPrimaryKey("dbo.TasksModels");
            AlterColumn("dbo.TaskToUsers", "UserIdInt", c => c.Int());
            AlterColumn("dbo.TaskToUsers", "UserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.TasksModels", "TasksModelId");
            CreateIndex("dbo.TaskToUsers", "UserId");
            AddForeignKey("dbo.TaskToUsers", "TasksModelId", "dbo.TasksModels", "TasksModelId");
            DropColumn("dbo.TaskToUsers", "TaskId");           
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskToUsers", "TaskId", c => c.Int(nullable: false));
            AddColumn("dbo.TasksModels", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.TaskToUsers", "TasksModelId", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "UserId" });
            DropPrimaryKey("dbo.TasksModels");
            AlterColumn("dbo.TaskToUsers", "UserId", c => c.String());
            AlterColumn("dbo.TaskToUsers", "UserIdInt", c => c.Int(nullable: false));
            DropColumn("dbo.TasksModels", "TasksModelId");
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_TasksModelId", newName: "IX_TaskModel_Id");
            RenameColumn(table: "dbo.TaskToUsers", name: "TasksModelId", newName: "TaskModel_Id");
            RenameColumn(table: "dbo.TaskToUsers", name: "UserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.TaskToUsers", "UserId", c => c.String());
            CreateIndex("dbo.TaskToUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels", "Id");
        }
    }
}
