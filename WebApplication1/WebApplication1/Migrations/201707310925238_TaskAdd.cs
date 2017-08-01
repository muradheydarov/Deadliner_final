namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskAdd : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TaskAssigneds", newName: "TaskToUsers");
            AddColumn("dbo.TaskToUsers", "TaskId", c => c.Int(nullable: false));
            AlterColumn("dbo.TaskToUsers", "UserIdInt", c => c.Int(nullable: false));
            DropColumn("dbo.TaskToUsers", "TaskModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskToUsers", "TaskModel_Id", c => c.Int());
            AlterColumn("dbo.TaskToUsers", "UserIdInt", c => c.String());
            DropColumn("dbo.TaskToUsers", "TaskId");
            RenameTable(name: "dbo.TaskToUsers", newName: "TaskAssigneds");
        }
    }
}
