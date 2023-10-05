-- Drop existing database if it exists
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'AcademicBlogDB')
BEGIN
    ALTER DATABASE AcademicBlogDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AcademicBlogDB;
END

-- Create a new database
CREATE DATABASE AcademicBlogDB;

-- Use the newly created database
USE AcademicBlogDB;

-- Disable foreign key checks (not applicable in SQL Server)

-- Drop existing tables if they exist
IF OBJECT_ID('Account', 'U') IS NOT NULL DROP TABLE [Account];
IF OBJECT_ID('Role', 'U') IS NOT NULL DROP TABLE [Role];
IF OBJECT_ID('Tag', 'U') IS NOT NULL DROP TABLE [Tag];
IF OBJECT_ID('Post', 'U') IS NOT NULL DROP TABLE [Post];
IF OBJECT_ID('PostTag', 'U') IS NOT NULL DROP TABLE [PostTag];
IF OBJECT_ID('Favourite', 'U') IS NOT NULL DROP TABLE [Favourite];
IF OBJECT_ID('Comment', 'U') IS NOT NULL DROP TABLE [Comment];
IF OBJECT_ID('Bookmark', 'U') IS NOT NULL DROP TABLE [Bookmark];
IF OBJECT_ID('Notification', 'U') IS NOT NULL DROP TABLE [Notification];
IF OBJECT_ID('Hit', 'U') IS NOT NULL DROP TABLE [Hit];
IF OBJECT_ID('Approve', 'U') IS NOT NULL DROP TABLE [Approve];
IF OBJECT_ID('Category', 'U') IS NOT NULL DROP TABLE [Category];

-- Re-create tables

-- Your CREATE TABLE statements go here

-- Add foreign key constraints

-- Your ALTER TABLE statements go here
-- Disable foreign key checks (not applicable in SQL Server)
-- DROP TABLE IF EXISTS is not available in SQL Server

-- Re-create tables

CREATE TABLE [Account] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(20) NOT NULL,
    [Email] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(20) NOT NULL,
    [Fullname] NVARCHAR(50) NOT NULL,
    [AvatarUrl] NVARCHAR(MAX) NOT NULL,
    [RoleID] INT NOT NULL,
    CONSTRAINT [UC_Username_Email] UNIQUE ([Username], [Email])
);

CREATE TABLE [Role] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(10) NOT NULL
);

CREATE TABLE [Tag] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(20) NOT NULL
);

CREATE TABLE [Post] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(100) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [ModifiedDate] DATETIME NOT NULL,
    [ThumbnailUrl] NVARCHAR(255) NOT NULL,
    [IsPublic] BIT NOT NULL,
    [CreatorId] INT NOT NULL,
    [CategoryID] INT NOT NULL,
    [ApproverID] INT NOT NULL,
    [ApproveDate] DATETIME NOT NULL
);

CREATE TABLE [PostTag] (
    [PostID] INT NOT NULL,
    [TagID] INT NOT NULL
);

CREATE TABLE [Favourite] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PostID] INT NOT NULL,
    [CreatorID] INT NOT NULL
);

CREATE TABLE [Comment] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [ModifiedDate] DATETIME NOT NULL,
    [PostID] INT NOT NULL,
    [CreatorID] INT NOT NULL,
    [ParentID] INT NOT NULL,
    [Path] NVARCHAR(255) NOT NULL
);

CREATE TABLE [Bookmark] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PostID] INT NOT NULL,
    [CreatorID] INT NOT NULL
);

CREATE TABLE [Notification] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Type] NVARCHAR(10) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [ReceiverID] INT NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [PostID] INT NOT NULL
);

CREATE TABLE [Hit] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [SessionID] NVARCHAR(50) NOT NULL,
    [PostID] INT NOT NULL,
    CONSTRAINT [UC_SessionID] UNIQUE ([SessionID])
);

CREATE TABLE [Category] (
    [ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(20) NOT NULL
);

-- Add foreign key constraints

ALTER TABLE [Account] ADD CONSTRAINT [FK_Account_Role] FOREIGN KEY ([RoleID]) REFERENCES [Role]([ID]);
ALTER TABLE [Post] ADD CONSTRAINT [FK_Post_Account] FOREIGN KEY ([CreatorId]) REFERENCES [Account]([ID]);
ALTER TABLE [Post] ADD CONSTRAINT [FK_Post_Category] FOREIGN KEY ([CategoryID]) REFERENCES [Category]([ID]);
ALTER TABLE [Post] ADD CONSTRAINT [FK_Post_Approver] FOREIGN KEY ([ApproverID]) REFERENCES [Account]([ID]);
ALTER TABLE [PostTag] ADD CONSTRAINT [FK_PostTag_Tag] FOREIGN KEY ([TagID]) REFERENCES [Tag]([ID]);
ALTER TABLE [PostTag] ADD CONSTRAINT [FK_PostTag_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);
ALTER TABLE [Favourite] ADD CONSTRAINT [FK_Favourite_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);
ALTER TABLE [Comment] ADD CONSTRAINT [FK_Comment_Account] FOREIGN KEY ([CreatorID]) REFERENCES [Account]([ID]);
ALTER TABLE [Comment] ADD CONSTRAINT [FK_Comment_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);
ALTER TABLE [Bookmark] ADD CONSTRAINT [FK_Bookmark_Account] FOREIGN KEY ([CreatorID]) REFERENCES [Account]([ID]);
ALTER TABLE [Bookmark] ADD CONSTRAINT [FK_Bookmark_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);
ALTER TABLE [Notification] ADD CONSTRAINT [FK_Notification_Account] FOREIGN KEY ([ReceiverID]) REFERENCES [Account]([ID]);
ALTER TABLE [Notification] ADD CONSTRAINT [FK_Notification_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);
ALTER TABLE [Hit] ADD CONSTRAINT [FK_Hit_Post] FOREIGN KEY ([PostID]) REFERENCES [Post]([ID]);

