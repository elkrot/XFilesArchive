/*IF EXISTS (
  SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE SPECIFIC_SCHEMA = N'dbo'
     AND SPECIFIC_NAME = N'TruncateTables' 
)
   DROP PROCEDURE [dbo].[TruncateTables]

   go*/

Create PROCEDURE [dbo].[TruncateTables] 
AS
BEGIN

BEGIN TRANSACTION;
	BEGIN TRY
 	delete FROM dbo.ImageToEntity
    delete FROM [dbo].[Image]
	DBCC CHECKIDENT ('dbo.Image' , RESEED, 0) 
    delete FROM dbo.ArchiveEntity 
	DBCC CHECKIDENT ('dbo.ArchiveEntity' , RESEED, 0) 
	delete FROM dbo.Drive
    DBCC CHECKIDENT ('dbo.Drive' , RESEED, 0) 
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
