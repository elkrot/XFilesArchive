CREATE TABLE [dbo].[CategoryToEntity] (
    [TargetEntityKey] INT NOT NULL,
    [CategoryKey]     INT NOT NULL,
    CONSTRAINT [PK_CategoryToEntity] PRIMARY KEY CLUSTERED ([TargetEntityKey] ASC, [CategoryKey] ASC),
    CONSTRAINT [FK_CategoryToEntity_ArchiveEntity] FOREIGN KEY ([TargetEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]),
    CONSTRAINT [FK_CategoryToEntity_Category] FOREIGN KEY ([CategoryKey]) REFERENCES [dbo].[Category] ([CategoryKey])
);

