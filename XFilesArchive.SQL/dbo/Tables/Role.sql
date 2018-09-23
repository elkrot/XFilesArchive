CREATE TABLE [dbo].[Role] (
    [RoleId]    INT           IDENTITY (1, 1) NOT NULL,
    [RoleTitle] NVARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_RoleTitleUnique_Column]
    ON [dbo].[Role]([RoleTitle] ASC);

