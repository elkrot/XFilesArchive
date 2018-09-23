CREATE TABLE [dbo].[Drive] (
    [DriveId]     INT             IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100)  NOT NULL,
    [HashCode]    INT             NOT NULL,
    [DriveInfo]   VARBINARY (MAX) NOT NULL,
    [CreatedDate] DATETIME2 (7)   CONSTRAINT [DF_Drive_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [DriveCode]   NCHAR (20)      NOT NULL,
    [IsSecret]    BIT             CONSTRAINT [DF_Drive_IsSecret] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Drive] PRIMARY KEY CLUSTERED ([DriveId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_DriveCode_Unique]
    ON [dbo].[Drive]([DriveCode] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Drive_Title_Unique]
    ON [dbo].[Drive]([Title] ASC);

