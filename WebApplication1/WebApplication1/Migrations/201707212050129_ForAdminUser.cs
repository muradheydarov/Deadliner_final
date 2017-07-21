namespace DeadLiner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForAdminUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [Surname], [UserStatus]) VALUES (N'27909fd6-4321-43fb-8744-c650ce91f5d0', N'hmurad.p101@code.edu.az', 1, N'AFEKA0eoac4s5as1GhBy0qV0YocyImZ4XAn3TFkPz31L5vSNTDhSD+jAqBAaXkL4Yg==', N'80393b18-1f79-4123-abd6-581c36853e9c', NULL, 0, 0, NULL, 1, 0, N'muradcode', NULL, NULL, NULL)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [Surname], [UserStatus]) VALUES (N'4ecc27f3-4a2d-48da-9719-0b731beb3887', N'arahim.p101@code.edu.az', 1, N'AEEnn6Qf8BmctAnKqEVSMcTQyT1bfYnRH3G/+Xpo0/yw30LH4p89lroFbW4v3avl5A==', N'639cbb69-7908-4648-b277-2d39dcddc687', NULL, 0, 0, NULL, 1, 0, N'rehimkisi', NULL, NULL, NULL)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [Surname], [UserStatus]) VALUES (N'efa72772-8aa8-4478-bd0f-e4321447a777', N'muradheyderov@gmail.com', 1, N'AKoYUJIl5BxEYKTr00k3vNxQEnfCwxLbrMrOPWy3XHF69syz9ubdaCr4RkARL4DGmQ==', N'31c0b7dd-e00a-46d1-8bff-1a43fa5bdc2d', NULL, 0, 0, NULL, 1, 0, N'Admin', N'Admin', N'Admin', N'Student')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8e264c29-c4ab-43a8-86cd-4432120d8cca', N'Admin')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'efa72772-8aa8-4478-bd0f-e4321447a777', N'8e264c29-c4ab-43a8-86cd-4432120d8cca')

");
        }
        
        public override void Down()
        {
        }
    }
}
