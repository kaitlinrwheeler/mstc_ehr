-- Insert Roles
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('1', 'Admin', 'ADMIN', NEWID());

-- Insert Roles
INSERT INTO [dbo].AspNetRoles ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('2', 'Student', 'STUDENT', NEWID());

-- Insert into User
INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [Discriminator], [Firstname], [Lastname])
VALUES ('7c986f13-f522-458b-8b35-eca379c3052a', 'EHRAdmin@mstc.edu', 'EHRADMIN@MSTC.EDU', 0, 'AQAAAAIAAYagAAAAEIsaL0cHnefLRKhOJPLRods/PZL5NpuN6jHh7zdkILLXP980pE80tkLbzMCpnZxdaw==', 'YRF2CJAJ2POLC7CZTQYYU3UOW5YNG7CG', 
'6b0712e4-bbf5-4777-9f14-3494f8491b18', 0, 0, 1, 0, 'ApplicationUser', 'Admin', 'EHR');

-- Insert into User Roles
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
VALUES ('7c986f13-f522-458b-8b35-eca379c3052a', 1);