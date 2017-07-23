namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAnotherIdForAspNetUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUserId", c => c.Int(nullable: false, identity: true));
            Sql(@"
ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
ALTER TABLE dbo.[AspNetUsers] DROP CONSTRAINT [PK_dbo.AspNetUsers]
--Uncomment this line in case you need to modify this table again
--ALTER TABLE dbo.[AspNetUsers] DROP CONSTRAINT [dbo.AspNetUsers_ApplicationUserId]

-- Add the constraints back, but now nonclustered
ALTER TABLE dbo.[AspNetUsers] add constraint [PK_dbo.AspNetUsers] primary key     nonclustered 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

-- Add the other constraints back
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

-- Add NEW constraint with unique clustered
ALTER TABLE dbo.[AspNetUsers] add constraint [dbo.AspNetUsers_ApplicationUserId] unique clustered 
(
[ApplicationUserId]
)
");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ApplicationUserId");
        }
    }
}
