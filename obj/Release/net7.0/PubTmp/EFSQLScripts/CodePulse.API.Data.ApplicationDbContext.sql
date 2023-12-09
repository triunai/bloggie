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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006064940_try')
BEGIN
    CREATE TABLE [BlogPost] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [ShortDescription] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [FeaturedImageUrl] nvarchar(max) NOT NULL,
        [UrlHandle] nvarchar(max) NOT NULL,
        [PublishedDate] datetime2 NOT NULL,
        [Author] nvarchar(max) NOT NULL,
        [IsVisible] bit NOT NULL,
        CONSTRAINT [PK_BlogPost] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006064940_try')
BEGIN
    CREATE TABLE [Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [UrlHandle] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006064940_try')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231006064940_try', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016050134_UpdateSchema')
BEGIN
    ALTER TABLE [BlogPost] ADD [CategoriesId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016050134_UpdateSchema')
BEGIN
    CREATE INDEX [IX_BlogPost_CategoriesId] ON [BlogPost] ([CategoriesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016050134_UpdateSchema')
BEGIN
    ALTER TABLE [BlogPost] ADD CONSTRAINT [FK_BlogPost_Categories_CategoriesId] FOREIGN KEY ([CategoriesId]) REFERENCES [Categories] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016050134_UpdateSchema')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231016050134_UpdateSchema', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    ALTER TABLE [BlogPost] DROP CONSTRAINT [FK_BlogPost_Categories_CategoriesId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    DROP INDEX [IX_BlogPost_CategoriesId] ON [BlogPost];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogPost]') AND [c].[name] = N'CategoriesId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [BlogPost] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [BlogPost] DROP COLUMN [CategoriesId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    CREATE TABLE [BlogPostCategories] (
        [BlogPostsId] uniqueidentifier NOT NULL,
        [CategoriesId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_BlogPostCategories] PRIMARY KEY ([BlogPostsId], [CategoriesId]),
        CONSTRAINT [FK_BlogPostCategories_BlogPost_BlogPostsId] FOREIGN KEY ([BlogPostsId]) REFERENCES [BlogPost] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BlogPostCategories_Categories_CategoriesId] FOREIGN KEY ([CategoriesId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    CREATE INDEX [IX_BlogPostCategories_CategoriesId] ON [BlogPostCategories] ([CategoriesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231018050208_Added relationships, many-to-many')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231018050208_Added relationships, many-to-many', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231027030137_Added blogImages to domain models')
BEGIN
    CREATE TABLE [BlogImages] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [FileExtension] nvarchar(max) NOT NULL,
        [FileUrl] nvarchar(max) NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        CONSTRAINT [PK_BlogImages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231027030137_Added blogImages to domain models')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231027030137_Added blogImages to domain models', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231206121858_categoryJson added')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231206121858_categoryJson added', N'7.0.13');
END;
GO

COMMIT;
GO

