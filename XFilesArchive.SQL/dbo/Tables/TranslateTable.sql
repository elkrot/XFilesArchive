CREATE TABLE [dbo].[TranslateTable] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [LanguageCode] NCHAR (5)      NOT NULL,
    [Title]        NVARCHAR (500) NULL,
    [Translate]    NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

