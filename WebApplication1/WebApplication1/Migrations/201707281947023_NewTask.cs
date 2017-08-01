namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTask : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskAssigneds", name: "TaskModel_Id", newName: "TasksModel_Id");
            RenameIndex(table: "dbo.TaskAssigneds", name: "IX_TaskModel_Id", newName: "IX_TasksModel_Id");
            AddColumn("dbo.TaskAssigneds", "TaskModelId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskAssigneds", "TaskModelId");
            RenameIndex(table: "dbo.TaskAssigneds", name: "IX_TasksModel_Id", newName: "IX_TaskModel_Id");
            RenameColumn(table: "dbo.TaskAssigneds", name: "TasksModel_Id", newName: "TaskModel_Id");
        }
    }
}
