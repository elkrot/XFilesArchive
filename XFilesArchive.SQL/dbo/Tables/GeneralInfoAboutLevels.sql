CREATE TABLE [dbo].[GeneralInfoAboutLevels] (
    [Level]    SMALLINT       NOT NULL,
    [Generic]  NVARCHAR (100) NULL,
    [Audio]    NVARCHAR (100) NULL,
    [Video]    NVARCHAR (100) NULL,
    [Images]   NVARCHAR (100) NULL,
    [Examples] NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Level] ASC)
);

