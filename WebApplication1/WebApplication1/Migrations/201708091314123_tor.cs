namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "TaskModel_Id" });
            RenameColumn(table: "dbo.TaskToUsers", name: "TaskModel_Id", newName: "TasksModelID");
            DropPrimaryKey("dbo.TasksModels");
            AddColumn("dbo.TasksModels", "TasksModelID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.TaskToUsers", "TasksModelID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TasksModels", "TasksModelID");
            CreateIndex("dbo.TaskToUsers", "TasksModelID");
            AddForeignKey("dbo.TaskToUsers", "TasksModelID", "dbo.TasksModels", "TasksModelID", cascadeDelete: true);
            DropColumn("dbo.TasksModels", "Id");
            DropColumn("dbo.TaskToUsers", "TaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskToUsers", "TaskId", c => c.Int(nullable: false));
            AddColumn("dbo.TasksModels", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.TaskToUsers", "TasksModelID", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "TasksModelID" });
            DropPrimaryKey("dbo.TasksModels");
            AlterColumn("dbo.TaskToUsers", "TasksModelID", c => c.Int());
            DropColumn("dbo.TasksModels", "TasksModelID");
            AddPrimaryKey("dbo.TasksModels", "Id");
            RenameColumn(table: "dbo.TaskToUsers", name: "TasksModelID", newName: "TaskModel_Id");
            CreateIndex("dbo.TaskToUsers", "TaskModel_Id");
            AddForeignKey("dbo.TaskToUsers", "TaskModel_Id", "dbo.TasksModels", "Id");
        }
    }
}
