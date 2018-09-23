CREATE TABLE [dbo].[Category] (
    [CategoryKey]       INT            IDENTITY (1, 1) NOT NULL,
    [CategoryTitle]     NVARCHAR (100) NULL,
    [ParentCategoryKey] INT            NULL,
    [CreatedDate]       DATETIME2 (7)  CONSTRAINT [DF_Category_CreatedDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryKey] ASC)
);

