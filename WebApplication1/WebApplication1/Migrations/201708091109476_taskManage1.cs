namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskManage1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskToUsers", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_UserId", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TaskToUsers", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.TaskToUsers", name: "User_Id", newName: "UserId");
        }
    }
}
