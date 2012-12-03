CREATE PROCEDURE [dbo].[Wei_GetLock]
	@RequestId INT 
AS

/* Stored Procedure Wei_GetLock
	   
	   History:
	   Modified Date		Author		Change
	   
*/
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
	
	SET NOCOUNT ON
	
	SELECT @ErrorMsg = 'Update to ProcessLock column on Requests table failed', @TableName = 'Requests'
	
	BEGIN TRY
		BEGIN TRANSACTION Wei_GetLock
		UPDATE	Requests 
		SET		ProcessLock=1, 
				ModifiedDatetime=GETDATE(), 
				ModifiedOper='dbo' 
		WHERE	ProcessLock=0 
		AND		Id=@RequestId

		COMMIT TRANSACTION Wei_GetLock
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_GetLock
		RAISERROR (@ErrorMsg,16,1)
	END CATCH
	RETURN 0
GO