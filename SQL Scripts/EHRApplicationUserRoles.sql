-- Insert Roles
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('1', 'Admin', 'ADMIN', NEWID()),
('2', 'Student', 'STUDENT', NEWID());

-- Insert into User. This will create a backdoor admin account
INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [Discriminator], [Firstname], [Lastname])
VALUES ('7c986f13-f522-458b-8b35-eca379c3052a', 'EHRAdmin@mstc.edu', 'EHRADMIN@MSTC.EDU', 0, 'AQAAAAIAAYagAAAAEIsaL0cHnefLRKhOJPLRods/PZL5NpuN6jHh7zdkILLXP980pE80tkLbzMCpnZxdaw==', 'YRF2CJAJ2POLC7CZTQYYU3UOW5YNG7CG', 
'6b0712e4-bbf5-4777-9f14-3494f8491b18', 0, 0, 1, 0, 'ApplicationUser', 'Admin', 'EHR'),--This is for a backdoor Admin account.

('f04cb17b-ba5d-4da5-a477-6ce8a3ffafd8', 'Julie.Larsen@mstc.edu', 'JULIE.LARSEN@MSTC.EDU', 0, 'AQAAAAIAAYagAAAAEGunlBElGY1sXFzcpPYLHYZZgJmbUDV3ezXMCZiKtChR/OMhT4Ih2EK2Om69pdCyqw==', 'WTAHTIKJTUMJPRLOTANSUBWH7F7N76RJ', 
'8b3c4464-578c-4151-bcd6-fa4b0e11d3b1', 0, 0, 1, 0, 'ApplicationUser', 'Julie', 'Larsen'),--This is Julies Admin account.

('508842ed-1330-43cb-b900-17f3ca651659', 'Brent.Presley@mstc.edu', 'BRENT.PRESLEY@MSTC.EDU', 0, 'AQAAAAIAAYagAAAAECm1intuKekp9xL8ALyXGZ/ekuS+KquUG9M+DQmFaa0xOoRHqDe7oOW+Q2dQ9llVeA==', 'OVEKXSAZNLZUICXBZIG2MNUPJLVJ2K5F', 
'1c30de31-06e7-4eac-b846-1658070173bf', 0, 0, 1, 0, 'ApplicationUser', 'Brent', 'Presley');--This is Brents Admin account.

-- Insert into User Roles
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
VALUES ('7c986f13-f522-458b-8b35-eca379c3052a', 1),--This is a backdoor Admin account.

('f04cb17b-ba5d-4da5-a477-6ce8a3ffafd8', 1),--This is Julies Admin account.

('508842ed-1330-43cb-b900-17f3ca651659', 1);--This is Brents Admin account.

