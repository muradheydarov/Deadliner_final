namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'076e1076-06ba-42ab-95e0-47be30c90353', N'admin@gmail.com', 0, N'AFNlqQgLgn17hpG24wZmg9pLFsHW87pPENGyxDwxe3jPcsVVl4ky/fMlPCJvA+7MNA==', N'afa4adc4-92bb-4b75-b846-ca9ae111aae7', NULL, 0, 0, NULL, 1, 0, N'admin')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'7aad7afb-f687-462e-bb01-835765029dc5', N'guest@gmail.com', 0, N'AG/+Llr874YGtUrgkBVkWtG8se9Y0oDIk/OhRDVNO2+PCe1/H38LSyWGfFtbwG8mhQ==', N'5543781e-6e5f-451d-9ed2-c6ad5e75ffca', NULL, 0, 0, NULL, 1, 0, N'guest')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'a0100bd1-1743-4396-a3a9-4f9ac2800437', N'Admin')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'076e1076-06ba-42ab-95e0-47be30c90353', N'a0100bd1-1743-4396-a3a9-4f9ac2800437')
");
        }
        
        public override void Down()
        {
        }
    }
}
