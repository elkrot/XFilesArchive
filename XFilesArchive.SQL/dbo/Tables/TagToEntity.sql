CREATE TABLE [dbo].[TagToEntity] (
    [TargetEntityKey] INT NOT NULL,
    [TagKey]          INT NOT NULL,
    CONSTRAINT [PK_TagToEntity] PRIMARY KEY CLUSTERED ([TargetEntityKey] ASC, [TagKey] ASC),
    CONSTRAINT [FK_TagToEntity_ArchiveEntity] FOREIGN KEY ([TargetEntityKey]) REFERENCES [dbo].[ArchiveEntity] ([ArchiveEntityKey]),
    CONSTRAINT [FK_TagToEntity_Tag] FOREIGN KEY ([TagKey]) REFERENCES [dbo].[Tag] ([TagKey])
);

