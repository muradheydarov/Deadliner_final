namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskModels",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Heading = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        DeadlineTask = c.Int(nullable:false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.String(),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskModels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TaskModels", new[] { "User_Id" });
            DropTable("dbo.TaskModels");
        }
    }
}
