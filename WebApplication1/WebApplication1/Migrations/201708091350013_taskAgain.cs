namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TasksModels",
                c => new
                    {
                        TasksModelID = c.Int(nullable: false, identity: true),
                        Heading = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.TasksModelID);
            
            CreateTable(
                "dbo.TaskToUsers",
                c => new
                    {
                        TaskToUserID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        TasksModelID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskToUserID)
                .ForeignKey("dbo.TasksModels", t => t.TasksModelID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.TasksModelID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskToUsers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TaskToUsers", "TasksModelID", "dbo.TasksModels");
            DropIndex("dbo.TaskToUsers", new[] { "TasksModelID" });
            DropIndex("dbo.TaskToUsers", new[] { "Id" });
            DropTable("dbo.TaskToUsers");
            DropTable("dbo.TasksModels");
        }
    }
}
