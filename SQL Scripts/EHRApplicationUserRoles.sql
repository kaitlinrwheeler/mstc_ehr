-- Insert Admin role
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('1', 'Admin', 'ADMIN', NEWID());

-- Insert Student role
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES ('2', 'Student', 'STUDENT', NEWID());
