IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Category] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK__Category__3214EC27489DE8C0] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Role] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(10) NOT NULL,
    CONSTRAINT [PK__Role__3214EC27E8245A8B] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Tag] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    CONSTRAINT [PK__Tag__3214EC27A0DFC654] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Account] (
    [ID] int NOT NULL IDENTITY,
    [Username] nvarchar(20) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Password] nvarchar(20) NOT NULL,
    [Fullname] nvarchar(50) NOT NULL,
    [AvatarUrl] nvarchar(max) NOT NULL,
    [RoleID] int NOT NULL,
    CONSTRAINT [PK__Account__3214EC277C099CDF] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Account_Role] FOREIGN KEY ([RoleID]) REFERENCES [Role] ([ID])
);
GO

CREATE TABLE [Post] (
    [ID] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [CreatedDate] datetime NOT NULL,
    [ModifiedDate] datetime NOT NULL,
    [ThumbnailUrl] nvarchar(255) NOT NULL,
    [IsPublic] bit NOT NULL,
    [CreatorId] int NOT NULL,
    [CategoryID] int NOT NULL,
    [ApproverID] int NOT NULL,
    [ApproveDate] datetime NOT NULL,
    CONSTRAINT [PK__Post__3214EC27773A454B] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Post_Account] FOREIGN KEY ([CreatorId]) REFERENCES [Account] ([ID]),
    CONSTRAINT [FK_Post_Approver] FOREIGN KEY ([ApproverID]) REFERENCES [Account] ([ID]),
    CONSTRAINT [FK_Post_Category] FOREIGN KEY ([CategoryID]) REFERENCES [Category] ([ID])
);
GO

CREATE TABLE [Bookmark] (
    [ID] int NOT NULL IDENTITY,
    [PostID] int NOT NULL,
    [CreatorID] int NOT NULL,
    CONSTRAINT [PK__Bookmark__3214EC27B06F2739] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Bookmark_Account] FOREIGN KEY ([CreatorID]) REFERENCES [Account] ([ID]),
    CONSTRAINT [FK_Bookmark_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID])
);
GO

CREATE TABLE [Comment] (
    [ID] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CreatedDate] datetime NOT NULL,
    [ModifiedDate] datetime NOT NULL,
    [PostID] int NOT NULL,
    [CreatorID] int NOT NULL,
    [ParentID] int NOT NULL,
    [Path] nvarchar(255) NOT NULL,
    CONSTRAINT [PK__Comment__3214EC27EBE64D02] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Comment_Account] FOREIGN KEY ([CreatorID]) REFERENCES [Account] ([ID]),
    CONSTRAINT [FK_Comment_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID])
);
GO

CREATE TABLE [Favourite] (
    [ID] int NOT NULL IDENTITY,
    [PostID] int NOT NULL,
    [CreatorID] int NOT NULL,
    CONSTRAINT [PK__Favourit__3214EC27C7CF2A6D] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Favourite_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID])
);
GO

CREATE TABLE [Hit] (
    [ID] int NOT NULL IDENTITY,
    [SessionID] nvarchar(50) NOT NULL,
    [PostID] int NOT NULL,
    CONSTRAINT [PK__Hit__3214EC270302DCA0] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Hit_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID])
);
GO

CREATE TABLE [Notification] (
    [ID] int NOT NULL IDENTITY,
    [Type] nvarchar(10) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [ReceiverID] int NOT NULL,
    [CreatedDate] datetime NOT NULL,
    [PostID] int NOT NULL,
    CONSTRAINT [PK__Notifica__3214EC27DD018ABC] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Notification_Account] FOREIGN KEY ([ReceiverID]) REFERENCES [Account] ([ID]),
    CONSTRAINT [FK_Notification_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID])
);
GO

CREATE TABLE [PostTag] (
    [PostID] int NOT NULL,
    [TagID] int NOT NULL,
    CONSTRAINT [FK_PostTag_Post] FOREIGN KEY ([PostID]) REFERENCES [Post] ([ID]),
    CONSTRAINT [FK_PostTag_Tag] FOREIGN KEY ([TagID]) REFERENCES [Tag] ([ID])
);
GO

CREATE INDEX [IX_Account_RoleID] ON [Account] ([RoleID]);
GO

CREATE UNIQUE INDEX [UC_Username_Email] ON [Account] ([Username], [Email]);
GO

CREATE INDEX [IX_Bookmark_CreatorID] ON [Bookmark] ([CreatorID]);
GO

CREATE INDEX [IX_Bookmark_PostID] ON [Bookmark] ([PostID]);
GO

CREATE INDEX [IX_Comment_CreatorID] ON [Comment] ([CreatorID]);
GO

CREATE INDEX [IX_Comment_PostID] ON [Comment] ([PostID]);
GO

CREATE INDEX [IX_Favourite_PostID] ON [Favourite] ([PostID]);
GO

CREATE INDEX [IX_Hit_PostID] ON [Hit] ([PostID]);
GO

CREATE UNIQUE INDEX [UC_SessionID] ON [Hit] ([SessionID]);
GO

CREATE INDEX [IX_Notification_PostID] ON [Notification] ([PostID]);
GO

CREATE INDEX [IX_Notification_ReceiverID] ON [Notification] ([ReceiverID]);
GO

CREATE INDEX [IX_Post_ApproverID] ON [Post] ([ApproverID]);
GO

CREATE INDEX [IX_Post_CategoryID] ON [Post] ([CategoryID]);
GO

CREATE INDEX [IX_Post_CreatorId] ON [Post] ([CreatorId]);
GO

CREATE INDEX [IX_PostTag_PostID] ON [PostTag] ([PostID]);
GO

CREATE INDEX [IX_PostTag_TagID] ON [PostTag] ([TagID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231007162733_init', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231007163226_init2', N'7.0.11');
GO

COMMIT;
GO

