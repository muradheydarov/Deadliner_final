namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskToUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskToUsers", "TasksModel_Id", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "User_Id" });
            DropColumn("dbo.TaskToUsers", "UserId");
            RenameColumn(table: "dbo.TaskToUsers", name: "TasksModel_Id", newName: "TasksModelId");
            RenameColumn(table: "dbo.TaskToUsers", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_TasksModel_Id", newName: "IX_TasksModelId");
            DropPrimaryKey("dbo.TasksModels");
            AddColumn("dbo.TasksModels", "TasksModelId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.TaskToUsers", "UserIdInt", c => c.Int());
            AlterColumn("dbo.TaskToUsers", "UserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.TasksModels", "TasksModelId");
            CreateIndex("dbo.TaskToUsers", "UserId");
            AddForeignKey("dbo.TaskToUsers", "TasksModelId", "dbo.TasksModels", "TasksModelId");
            DropColumn("dbo.TasksModels", "Id");
            DropColumn("dbo.TaskToUsers", "TaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskToUsers", "TaskId", c => c.Int());
            AddColumn("dbo.TasksModels", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.TaskToUsers", "TasksModelId", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "UserId" });
            DropPrimaryKey("dbo.TasksModels");
            AlterColumn("dbo.TaskToUsers", "UserId", c => c.Int());
            DropColumn("dbo.TaskToUsers", "UserIdInt");
            DropColumn("dbo.TasksModels", "TasksModelId");
            AddPrimaryKey("dbo.TasksModels", "Id");
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_TasksModelId", newName: "IX_TasksModel_Id");
            RenameColumn(table: "dbo.TaskToUsers", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.TaskToUsers", name: "TasksModelId", newName: "TasksModel_Id");
            AddColumn("dbo.TaskToUsers", "UserId", c => c.Int());
            CreateIndex("dbo.TaskToUsers", "User_Id");
            AddForeignKey("dbo.TaskToUsers", "TasksModel_Id", "dbo.TasksModels", "Id");
        }
    }
}
