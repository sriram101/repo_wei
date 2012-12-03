CREATE PROCEDURE [dbo].[Wei_MarkRequestError]
	@RequestId		INT
AS

	/* Stored Procedure Wei_MarkRequestError
		
	   @RequestId		-- 
	   
		History:
	   Modified Date		Author		Change
	   
	  */

	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
			
	--- ********************* Begin Rule Procedure **********************************
	SET NOCOUNT ON
	

	BEGIN TRY
		BEGIN TRANSACTION Wei_MarkRequestError
	
		SELECT @ErrorMsg = 'Update to IsError Column on Requests table failed', @TableName = 'Requests'

		UPDATE	Requests
		SET		IsError = 1
		WHERE	Id = @RequestId

		COMMIT TRANSACTION Wei_MarkRequestError

	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_MarkRequestError
		RAISERROR (@ErrorMsg,16,1)
		
	END CATCH
	--RETURN @@IDENTITY	
 GO