CREATE TABLE [dbo].[User] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Email]    NVARCHAR (50) NULL,
    [Password] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK__User__1788CC4C5812160E] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserNameUnique]
    ON [dbo].[User]([Username] ASC);

