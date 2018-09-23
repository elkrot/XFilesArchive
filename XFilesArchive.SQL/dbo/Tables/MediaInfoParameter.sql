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

