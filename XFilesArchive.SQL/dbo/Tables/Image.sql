CREATE TABLE [dbo].[Image] (
    [ImageKey]      INT            IDENTITY (1, 1) NOT NULL,
    [Thumbnail]     IMAGE          NULL,
    [ImagePath]     NVARCHAR (255) NULL,
    [ThumbnailPath] NVARCHAR (255) NULL,
    [ImageTitle]    NVARCHAR (100) NULL,
    [HashCode]      INT            NULL,
    [CreatedDate]   DATETIME2 (7)  CONSTRAINT [DF_Image_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([ImageKey] ASC)
);

