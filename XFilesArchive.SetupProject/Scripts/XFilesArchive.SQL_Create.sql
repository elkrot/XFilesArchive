/*
Скрипт развертывания для XFilesArchive.SQL

Этот код был создан программным средством.
Изменения, внесенные в этот файл, могут привести к неверному выполнению кода и будут потеряны
в случае его повторного формирования.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "XFilesArchiveDB"
:setvar DefaultFilePrefix "XFilesArchiveDB"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
/*
Проверьте режим SQLCMD и отключите выполнение скрипта, если режим SQLCMD не поддерживается.
Чтобы повторно включить скрипт после включения режима SQLCMD выполните следующую инструкцию:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Для успешного выполнения этого скрипта должен быть включен режим SQLCMD.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Выполняется создание $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE Ukrainian_CI_AS
GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Параметры базы данных изменить нельзя. Применить эти параметры может только пользователь SysAdmin.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Параметры базы данных изменить нельзя. Применить эти параметры может только пользователь SysAdmin.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF),
                MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = OFF,
                DELAYED_DURABILITY = DISABLED 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_PLANS_PER_QUERY = 200, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE = OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
    END


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Выполняется создание [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Email]    NVARCHAR (50) NULL,
    [Password] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK__User__1788CC4C5812160E] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[User].[IX_UserNameUnique]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserNameUnique]
    ON [dbo].[User]([Username] ASC);


GO
PRINT N'Выполняется создание [dbo].[UserToRole]...';


GO
CREATE TABLE [dbo].[UserToRole] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [RoleId]    INT           IDENTITY (1, 1) NOT NULL,
    [RoleTitle] NVARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Role].[IX_RoleTitleUnique_Column]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_RoleTitleUnique_Column]
    ON [dbo].[Role]([RoleTitle] ASC);


GO
PRINT N'Выполняется создание [dbo].[Drive]...';


GO
CREATE TABLE [dbo].[Drive] (
    [DriveId]     INT             IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100)  NOT NULL,
    [HashCode]    INT             NOT NULL,
    [DriveInfo]   VARBINARY (MAX) NOT NULL,
    [CreatedDate] DATETIME2 (7)   NOT NULL,
    [DriveCode]   NCHAR (20)      NOT NULL,
    [IsSecret]    BIT             NOT NULL,
    CONSTRAINT [PK_Drive] PRIMARY KEY CLUSTERED ([DriveId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Drive].[IX_Drive_DriveCode_Unique]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_DriveCode_Unique]
    ON [dbo].[Drive]([DriveCode] ASC);


GO
PRINT N'Выполняется создание [dbo].[Drive].[IX_Drive_Title_Unique]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_Title_Unique]
    ON [dbo].[Drive]([Title] ASC);


GO
PRINT N'Выполняется создание [dbo].[GeneralInfoAboutLevels]...';


GO
CREATE TABLE [dbo].[GeneralInfoAboutLevels] (
    [Level]    SMALLINT       NOT NULL,
    [Generic]  NVARCHAR (100) NULL,
    [Audio]    NVARCHAR (100) NULL,
    [Video]    NVARCHAR (100) NULL,
    [Images]   NVARCHAR (100) NULL,
    [Examples] NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Level] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[MediaInfoParameter]...';


GO
CREATE TABLE [dbo].[MediaInfoParameter] (
    [MediaInfoParameterId] INT            IDENTITY (1, 1) NOT NULL,
    [MediaInfoTypeId]      INT            NOT NULL,
    [Title]                NVARCHAR (200) NOT NULL,
    [p1]                   NVARCHAR (200) NULL,
    [p2]                   NVARCHAR (200) NULL,
    [p3]                   NVARCHAR (200) NULL,
    [p4]                   NVARCHAR (200) NULL,
    [p5]                   NVARCHAR (200) NULL,
    [Description]          NVARCHAR (500) NULL,
    [p6]                   NVARCHAR (500) NULL,
    [p7]                   NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([MediaInfoParameterId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[MediaInfoType]...';


GO
CREATE TABLE [dbo].[MediaInfoType] (
    [MediaInfoTypeId] INT           NOT NULL,
    [Title]           NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([MediaInfoTypeId] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[TranslateTable]...';


GO
CREATE TABLE [dbo].[TranslateTable] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [LanguageCode] NCHAR (5)      NOT NULL,
    [Title]        NVARCHAR (500) NULL,
    [Translate]    NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Tag]...';


GO
CREATE TABLE [dbo].[Tag] (
    [TagKey]        INT           IDENTITY (1, 1) NOT NULL,
    [TagTitle]      NCHAR (50)    NOT NULL,
    [ModififedDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([TagKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Tag].[TagTitleUniq]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [TagTitleUniq]
    ON [dbo].[Tag]([TagTitle] ASC);


GO
PRINT N'Выполняется создание [dbo].[Category]...';


GO
CREATE TABLE [dbo].[Category] (
    [CategoryKey]       INT            IDENTITY (1, 1) NOT NULL,
    [CategoryTitle]     NVARCHAR (100) NULL,
    [ParentCategoryKey] INT            NULL,
    [CreatedDate]       DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[TagToEntity]...';


GO
CREATE TABLE [dbo].[TagToEntity] (
    [TargetEntityKey] INT NOT NULL,
    [TagKey]          INT NOT NULL,
    CONSTRAINT [PK_TagToEntity] PRIMARY KEY CLUSTERED ([TargetEntityKey] ASC, [TagKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[ImageToEntity]...';


GO
CREATE TABLE [dbo].[ImageToEntity] (
    [TargetEntityKey] INT NOT NULL,
    [ImageKey]        INT NOT NULL,
    CONSTRAINT [PK_ImageToEntity] PRIMARY KEY CLUSTERED ([TargetEntityKey] ASC, [ImageKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[CategoryToEntity]...';


GO
CREATE TABLE [dbo].[CategoryToEntity] (
    [TargetEntityKey] INT NOT NULL,
    [CategoryKey]     INT NOT NULL,
    CONSTRAINT [PK_CategoryToEntity] PRIMARY KEY CLUSTERED ([TargetEntityKey] ASC, [CategoryKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity]...';


GO
CREATE TABLE [dbo].[ArchiveEntity] (
    [ArchiveEntityKey] INT              IDENTITY (1, 1) NOT NULL,
    [ParentEntityKey]  INT              NULL,
    [DriveId]          INT              NOT NULL,
    [Title]            NVARCHAR (250)   NOT NULL,
    [EntityType]       TINYINT          NOT NULL,
    [EntityPath]       NVARCHAR (250)   NOT NULL,
    [EntityExtension]  NCHAR (20)       NULL,
    [Description]      XML              NULL,
    [FileSize]         BIGINT           NULL,
    [EntityInfo]       VARBINARY (MAX)  NULL,
    [CreatedDate]      DATETIME2 (7)    NOT NULL,
    [MFileInfo]        VARBINARY (MAX)  NULL,
    [Checksum]         NVARCHAR (MAX)   NULL,
    [Grade]            INT              NULL,
    [UniqGuid]         UNIQUEIDENTIFIER NULL,
    [ParentGuid]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ArchiveEntity] PRIMARY KEY CLUSTERED ([ArchiveEntityKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_ArchiveEntity_Title]...';


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_Title]
    ON [dbo].[ArchiveEntity]([Title] ASC);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_ArchiveEntity_FileSize]...';


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize]
    ON [dbo].[ArchiveEntity]([FileSize] ASC);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_ArchiveEntity_FileSize_Title]...';


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize_Title]
    ON [dbo].[ArchiveEntity]([FileSize] ASC, [Title] ASC);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_DriveId]...';


GO
CREATE NONCLUSTERED INDEX [IX_DriveId]
    ON [dbo].[ArchiveEntity]([DriveId] ASC);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_ArchiveEntity_Grade]...';


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_Grade]
    ON [dbo].[ArchiveEntity]([Grade] ASC);


GO
PRINT N'Выполняется создание [dbo].[ArchiveEntity].[IX_ArchiveEntity_UniqGuid]...';


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_UniqGuid]
    ON [dbo].[ArchiveEntity]([UniqGuid] ASC)
    INCLUDE([ParentGuid]);


GO
PRINT N'Выполняется создание [dbo].[Image]...';


GO
CREATE TABLE [dbo].[Image] (
    [ImageKey]      INT              IDENTITY (1, 1) NOT NULL,
    [Thumbnail]     IMAGE            NULL,
    [ImagePath]     NVARCHAR (255)   NULL,
    [ThumbnailPath] NVARCHAR (255)   NULL,
    [ImageTitle]    NVARCHAR (100)   NULL,
    [HashCode]      INT              NULL,
    [CreatedDate]   DATETIME2 (7)    NOT NULL,
    [UniqGuid]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([ImageKey] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[Image].[IX_Image_UniqGuid]...';


GO
CREATE NONCLUSTERED INDEX [IX_Image_UniqGuid]
    ON [dbo].[Image]([UniqGuid] ASC);


GO
PRINT N'Выполняется создание [dbo].[DF_Drive_CreatedDate]...';


GO
ALTER TABLE [dbo].[Drive]
    ADD CONSTRAINT [DF_Drive_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Выполняется создание [dbo].[DF_Drive_IsSecret]...';


GO
ALTER TABLE [dbo].[Drive]
    ADD CONSTRAINT [DF_Drive_IsSecret] DEFAULT ((0)) FOR [IsSecret];


GO
PRINT N'Выполняется создание [dbo].[DF_Tag_ModififedDate]...';


GO
ALTER TABLE [dbo].[Tag]
    ADD CONSTRAINT [DF_Tag_ModififedDate] DEFAULT (getdate()) FOR [ModififedDate];


GO
PRINT N'Выполняется создание [dbo].[DF_Category_CreatedDate]...';


GO
ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [DF_Category_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Выполняется создание [dbo].[DF_ArchiveEntity_CreatedDate]...';


GO
ALTER TABLE [dbo].[ArchiveEntity]
    ADD CONSTRAINT [DF_ArchiveEntity_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Выполняется создание [dbo].[DF_Image_CreatedDate]...';


GO
ALTER TABLE [dbo].[Image]
    ADD CONSTRAINT [DF_Image_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Выполняется создание [dbo].[FK_UserToRole_ToRole]...';


GO
ALTER TABLE [dbo].[UserToRole]
    ADD CONSTRAINT [FK_UserToRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([RoleId]);


GO
PRINT N'Выполняется создание [dbo].[FK_UserToRole_ToUser]...';


GO
ALTER TABLE [dbo].[UserToRole]
    ADD CONSTRAINT [FK_UserToRole_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]);


GO
PRINT N'Выполняется создание [dbo].[FK_TagToEntity_ArchiveEntity]...';


GO
ALTER TABLE [dbo].[TagToEntity]
    ADD CONSTRAINT [FK_TagToEntity_ArchiveEntity] FOREIGN KEY ([TargetEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_TagToEntity_Tag]...';


GO
ALTER TABLE [dbo].[TagToEntity]
    ADD CONSTRAINT [FK_TagToEntity_Tag] FOREIGN KEY ([TagKey]) REFERENCES [dbo].[Tag] ([TagKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_ImageToEntity_ArchiveEntity]...';


GO
ALTER TABLE [dbo].[ImageToEntity]
    ADD CONSTRAINT [FK_ImageToEntity_ArchiveEntity] FOREIGN KEY ([TargetEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_ImageToEntity_Image]...';


GO
ALTER TABLE [dbo].[ImageToEntity]
    ADD CONSTRAINT [FK_ImageToEntity_Image] FOREIGN KEY ([ImageKey]) REFERENCES [dbo].[Image] ([ImageKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_CategoryToEntity_ArchiveEntity]...';


GO
ALTER TABLE [dbo].[CategoryToEntity]
    ADD CONSTRAINT [FK_CategoryToEntity_ArchiveEntity] FOREIGN KEY ([TargetEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_CategoryToEntity_Category]...';


GO
ALTER TABLE [dbo].[CategoryToEntity]
    ADD CONSTRAINT [FK_CategoryToEntity_Category] FOREIGN KEY ([CategoryKey]) REFERENCES [dbo].[Category] ([CategoryKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_ArchiveEntity_ArchiveEntity]...';


GO
ALTER TABLE [dbo].[ArchiveEntity]
    ADD CONSTRAINT [FK_ArchiveEntity_ArchiveEntity] FOREIGN KEY ([ParentEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]);


GO
PRINT N'Выполняется создание [dbo].[FK_ArchiveEntity_Drive]...';


GO
ALTER TABLE [dbo].[ArchiveEntity]
    ADD CONSTRAINT [FK_ArchiveEntity_Drive] FOREIGN KEY ([DriveId]) REFERENCES [dbo].[Drive] ([DriveId]);


GO
PRINT N'Выполняется создание [dbo].[TruncateTables]...';


GO
/*IF EXISTS (
  SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE SPECIFIC_SCHEMA = N'dbo'
     AND SPECIFIC_NAME = N'TruncateTables' 
)
   DROP PROCEDURE [dbo].[TruncateTables]

   go*/

Create PROCEDURE [dbo].[TruncateTables] 
AS
BEGIN

BEGIN TRANSACTION;
	BEGIN TRY
 	delete FROM dbo.ImageToEntity
    delete FROM [dbo].[Image]
	DBCC CHECKIDENT ('dbo.Image' , RESEED, 0) 
    delete FROM dbo.ArchiveEntity 
	DBCC CHECKIDENT ('dbo.ArchiveEntity' , RESEED, 0) 
	delete FROM dbo.Drive
    DBCC CHECKIDENT ('dbo.Drive' , RESEED, 0) 
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
  end
GO
PRINT N'Выполняется создание [dbo].[DeleteDrive]...';


GO


CREATE PROCEDURE [dbo].[DeleteDrive] 
@DriveId int
AS
BEGIN
 


BEGIN TRANSACTION;

  BEGIN TRY
    -- Удаление записей об изображениях

delete t FROM ImageToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId

  delete i FROM [Image] i join ImageToEntity t on i.ImageKey = t.ImageKey join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey 
  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId
  
  delete  t FROM TagToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId

  delete t FROM CategoryToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId


delete from ArchiveEntity where isnull(DriveId,0)=@DriveId

delete from Drive where DriveId=@DriveId


END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
  end
GO
-- Выполняется этап рефакторинга для обновления развернутых журналов транзакций на целевом сервере

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'dd349e88-6f5f-4131-aec7-4499ba20957c')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('dd349e88-6f5f-4131-aec7-4499ba20957c')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '00c659b3-285c-403a-a69b-9bfcab537a4b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('00c659b3-285c-403a-a69b-9bfcab537a4b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'f29da845-00de-4de6-908d-57932a51973a')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('f29da845-00de-4de6-908d-57932a51973a')

GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
PRINT N'Обновление завершено.';


GO
