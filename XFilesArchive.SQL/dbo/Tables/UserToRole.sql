CREATE TABLE [dbo].[UserToRole] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserToRole_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([RoleId]),
    CONSTRAINT [FK_UserToRole_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

