

CREATE PROCEDURE [dbo].[DeleteDrive] 
@DriveId int
AS
BEGIN
 


BEGIN TRANSACTION;

  BEGIN TRY
    -- Удаление записей об изображениях

delete t FROM ImageToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId

  delete i FROM [Image] i join ImageToEntity t on i.ImageKey = t.ImageKey join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey 
  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId
  
  delete  t FROM TagToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId

  delete t FROM CategoryToEntity t join ArchiveEntity a on t.TargetEntityKey=a.ArchiveEntityKey  join Drive d on a.DriveId =d.DriveId 
  where d.DriveId=@DriveId


delete from ArchiveEntity where isnull(DriveId,0)=@DriveId

delete from Drive where DriveId=@DriveId


END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
  end