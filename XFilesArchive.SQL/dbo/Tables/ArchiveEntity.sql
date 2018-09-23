CREATE TABLE [dbo].[ArchiveEntity] (
    [ArchiveEntityKey] INT             IDENTITY (1, 1) NOT NULL,
    [ParentEntityKey]  INT             NULL,
    [DriveId]          INT             NULL,
    [Title]            NVARCHAR (250)  NOT NULL,
    [EntityType]       TINYINT         NOT NULL,
    [EntityPath]       NVARCHAR (250)  NOT NULL,
    [EntityExtension]  NCHAR (20)      NULL,
    [Description]      XML             NULL,
    [FileSize]         BIGINT          NULL,
    [EntityInfo]       VARBINARY (MAX) NULL,
    [CreatedDate]      DATETIME2 (7)   CONSTRAINT [DF_ArchiveEntity_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [MFileInfo]        VARBINARY (MAX) NULL,
    [Checksum]         NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_ArchiveEntity] PRIMARY KEY CLUSTERED ([ArchiveEntityKey] ASC),
    CONSTRAINT [FK_ArchiveEntity_ArchiveEntity] FOREIGN KEY ([ParentEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]),
    CONSTRAINT [FK_ArchiveEntity_Drive] FOREIGN KEY ([DriveId]) REFERENCES [dbo].[Drive] ([DriveId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_Title]
    ON [dbo].[ArchiveEntity]([Title] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize]
    ON [dbo].[ArchiveEntity]([FileSize] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ArchiveEntity_FileSize_Title]
    ON [dbo].[ArchiveEntity]([FileSize] ASC, [Title] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DriveId]
    ON [dbo].[ArchiveEntity]([DriveId] ASC);

