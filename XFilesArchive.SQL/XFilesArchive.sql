USE [master]
GO
/****** Object:  Database [HmeArhX]    Script Date: 20.05.2019 0:21:32 ******/
CREATE DATABASE [HmeArhX]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HmeArhX', FILENAME = N'D:\db\HmeArhX.mdf' , SIZE = 1072896KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HmeArhX_log', FILENAME = N'D:\db\HmeArhX_log.ldf' , SIZE = 92864KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HmeArhX] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HmeArhX].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HmeArhX] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HmeArhX] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HmeArhX] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HmeArhX] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HmeArhX] SET ARITHABORT OFF 
GO
ALTER DATABASE [HmeArhX] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [HmeArhX] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HmeArhX] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HmeArhX] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HmeArhX] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HmeArhX] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HmeArhX] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HmeArhX] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HmeArhX] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HmeArhX] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HmeArhX] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HmeArhX] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HmeArhX] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HmeArhX] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HmeArhX] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HmeArhX] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HmeArhX] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HmeArhX] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HmeArhX] SET  MULTI_USER 
GO
ALTER DATABASE [HmeArhX] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HmeArhX] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HmeArhX] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HmeArhX] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [HmeArhX] SET DELAYED_DURABILITY = DISABLED 
GO
USE [HmeArhX]
GO
/****** Object:  Table [dbo].[ArchiveEntity]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveEntity](
	[ArchiveEntityKey] [int] IDENTITY(1,1) NOT NULL,
	[ParentEntityKey] [int] NULL,
	[DriveId] [int] NULL,
	[Title] [nvarchar](250) NOT NULL,
	[EntityType] [tinyint] NOT NULL,
	[EntityPath] [nvarchar](250) NOT NULL,
	[EntityExtension] [nchar](20) NULL,
	[Description] [xml] NULL,
	[FileSize] [bigint] NULL,
	[EntityInfo] [varbinary](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_ArchiveEntity_CreatedDate]  DEFAULT (getdate()),
	[MFileInfo] [varbinary](max) NULL,
	[Checksum] [nvarchar](max) NULL,
	[Grade] [int] NULL,
	[UniqGuid] [uniqueidentifier] NULL,
	[ParentGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ArchiveEntity] PRIMARY KEY CLUSTERED 
(
	[ArchiveEntityKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Category]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryKey] [int] IDENTITY(1,1) NOT NULL,
	[CategoryTitle] [nvarchar](100) NULL,
	[ParentCategoryKey] [int] NULL,
	[CreatedDate] [datetime2](7) NULL CONSTRAINT [DF_Category_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoryToEntity]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryToEntity](
	[TargetEntityKey] [int] NOT NULL,
	[CategoryKey] [int] NOT NULL,
 CONSTRAINT [PK_CategoryToEntity] PRIMARY KEY CLUSTERED 
(
	[TargetEntityKey] ASC,
	[CategoryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Drive]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Drive](
	[DriveId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[HashCode] [int] NOT NULL,
	[DriveInfo] [varbinary](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_Drive_CreatedDate]  DEFAULT (getdate()),
	[DriveCode] [nchar](20) NOT NULL,
	[IsSecret] [bit] NOT NULL CONSTRAINT [DF_Drive_IsSecret]  DEFAULT ((0)),
 CONSTRAINT [PK_Drive] PRIMARY KEY CLUSTERED 
(
	[DriveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GeneralInfoAboutLevels]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralInfoAboutLevels](
	[Level] [smallint] NOT NULL,
	[Generic] [nvarchar](100) NULL,
	[Audio] [nvarchar](100) NULL,
	[Video] [nvarchar](100) NULL,
	[Images] [nvarchar](100) NULL,
	[Examples] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Level] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Image]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageKey] [int] IDENTITY(1,1) NOT NULL,
	[Thumbnail] [image] NULL,
	[ImagePath] [nvarchar](255) NULL,
	[ThumbnailPath] [nvarchar](255) NULL,
	[ImageTitle] [nvarchar](100) NULL,
	[HashCode] [int] NULL,
	[CreatedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_Image_CreatedDate]  DEFAULT (getdate()),
	[UniqGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[ImageKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageToEntity]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageToEntity](
	[TargetEntityKey] [int] NOT NULL,
	[ImageKey] [int] NOT NULL,
 CONSTRAINT [PK_ImageToEntity] PRIMARY KEY CLUSTERED 
(
	[TargetEntityKey] ASC,
	[ImageKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaInfoParameter]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaInfoParameter](
	[MediaInfoParameterId] [int] IDENTITY(1,1) NOT NULL,
	[MediaInfoTypeId] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[p1] [nvarchar](200) NULL,
	[p2] [nvarchar](200) NULL,
	[p3] [nvarchar](200) NULL,
	[p4] [nvarchar](200) NULL,
	[p5] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
	[p6] [nvarchar](500) NULL,
	[p7] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MediaInfoParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MediaInfoType]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaInfoType](
	[MediaInfoTypeId] [int] NOT NULL,
	[Title] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MediaInfoTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleTitle] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tag]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[TagKey] [int] IDENTITY(1,1) NOT NULL,
	[TagTitle] [nchar](50) NOT NULL,
	[ModififedDate] [datetime2](7) NOT NULL CONSTRAINT [DF_Tag_ModififedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[TagKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TagToEntity]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagToEntity](
	[TargetEntityKey] [int] NOT NULL,
	[TagKey] [int] NOT NULL,
 CONSTRAINT [PK_TagToEntity] PRIMARY KEY CLUSTERED 
(
	[TargetEntityKey] ASC,
	[TagKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TranslateTable]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TranslateTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LanguageCode] [nchar](5) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Translate] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FacebookId] [nchar](100) NULL,
	[Sid] [nchar](100) NULL,
 CONSTRAINT [PK__User__1788CC4C5812160E] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserToRole]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserToRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_ArchiveEntity_FileSize]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize] ON [dbo].[ArchiveEntity]
(
	[FileSize] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ArchiveEntity_FileSize_Title]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize_Title] ON [dbo].[ArchiveEntity]
(
	[FileSize] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ArchiveEntity_Grade]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_Grade] ON [dbo].[ArchiveEntity]
(
	[Grade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ArchiveEntity_Title]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_Title] ON [dbo].[ArchiveEntity]
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ArchiveEntity_UniqGuid]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_UniqGuid] ON [dbo].[ArchiveEntity]
(
	[UniqGuid] ASC
)
INCLUDE ( 	[ParentGuid]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DriveId]    Script Date: 20.05.2019 0:21:32 ******/
CREATE NONCLUSTERED INDEX [IX_DriveId] ON [dbo].[ArchiveEntity]
(
	[DriveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Drive_DriveCode_Unique]    Script Date: 20.05.2019 0:21:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_DriveCode_Unique] ON [dbo].[Drive]
(
	[DriveCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Drive_Title_Unique]    Script Date: 20.05.2019 0:21:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_Title_Unique] ON [dbo].[Drive]
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleTitleUnique_Column]    Script Date: 20.05.2019 0:21:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RoleTitleUnique_Column] ON [dbo].[Role]
(
	[RoleTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [TagTitleUniq]    Script Date: 20.05.2019 0:21:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [TagTitleUniq] ON [dbo].[Tag]
(
	[TagTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserNameUnique]    Script Date: 20.05.2019 0:21:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserNameUnique] ON [dbo].[User]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArchiveEntity]  WITH NOCHECK ADD  CONSTRAINT [FK_ArchiveEntity_ArchiveEntity] FOREIGN KEY([ParentEntityKey])
REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey])
GO
ALTER TABLE [dbo].[ArchiveEntity] CHECK CONSTRAINT [FK_ArchiveEntity_ArchiveEntity]
GO
ALTER TABLE [dbo].[ArchiveEntity]  WITH NOCHECK ADD  CONSTRAINT [FK_ArchiveEntity_Drive] FOREIGN KEY([DriveId])
REFERENCES [dbo].[Drive] ([DriveId])
GO
ALTER TABLE [dbo].[ArchiveEntity] CHECK CONSTRAINT [FK_ArchiveEntity_Drive]
GO
ALTER TABLE [dbo].[CategoryToEntity]  WITH CHECK ADD  CONSTRAINT [FK_CategoryToEntity_ArchiveEntity] FOREIGN KEY([TargetEntityKey])
REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey])
GO
ALTER TABLE [dbo].[CategoryToEntity] CHECK CONSTRAINT [FK_CategoryToEntity_ArchiveEntity]
GO
ALTER TABLE [dbo].[CategoryToEntity]  WITH CHECK ADD  CONSTRAINT [FK_CategoryToEntity_Category] FOREIGN KEY([CategoryKey])
REFERENCES [dbo].[Category] ([CategoryKey])
GO
ALTER TABLE [dbo].[CategoryToEntity] CHECK CONSTRAINT [FK_CategoryToEntity_Category]
GO
ALTER TABLE [dbo].[ImageToEntity]  WITH CHECK ADD  CONSTRAINT [FK_ImageToEntity_ArchiveEntity] FOREIGN KEY([TargetEntityKey])
REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey])
GO
ALTER TABLE [dbo].[ImageToEntity] CHECK CONSTRAINT [FK_ImageToEntity_ArchiveEntity]
GO
ALTER TABLE [dbo].[ImageToEntity]  WITH CHECK ADD  CONSTRAINT [FK_ImageToEntity_Image] FOREIGN KEY([ImageKey])
REFERENCES [dbo].[Image] ([ImageKey])
GO
ALTER TABLE [dbo].[ImageToEntity] CHECK CONSTRAINT [FK_ImageToEntity_Image]
GO
ALTER TABLE [dbo].[TagToEntity]  WITH CHECK ADD  CONSTRAINT [FK_TagToEntity_ArchiveEntity] FOREIGN KEY([TargetEntityKey])
REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey])
GO
ALTER TABLE [dbo].[TagToEntity] CHECK CONSTRAINT [FK_TagToEntity_ArchiveEntity]
GO
ALTER TABLE [dbo].[TagToEntity]  WITH CHECK ADD  CONSTRAINT [FK_TagToEntity_Tag] FOREIGN KEY([TagKey])
REFERENCES [dbo].[Tag] ([TagKey])
GO
ALTER TABLE [dbo].[TagToEntity] CHECK CONSTRAINT [FK_TagToEntity_Tag]
GO
ALTER TABLE [dbo].[UserToRole]  WITH CHECK ADD  CONSTRAINT [FK_UserToRole_ToRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UserToRole] CHECK CONSTRAINT [FK_UserToRole_ToRole]
GO
ALTER TABLE [dbo].[UserToRole]  WITH CHECK ADD  CONSTRAINT [FK_UserToRole_ToUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserToRole] CHECK CONSTRAINT [FK_UserToRole_ToUser]
GO
/****** Object:  StoredProcedure [dbo].[DeleteDrive]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[TruncateTables]    Script Date: 20.05.2019 0:21:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
USE [master]
GO
ALTER DATABASE [HmeArhX] SET  READ_WRITE 
GO
