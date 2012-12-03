IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_AddResponseMessage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_AddResponseMessage]
	@RequestId			INT, 
	@ResponseMessage	NVARCHAR(MAX),
	@Status				INT,
	@IsError			INT,
	@ModifiedOper		VARCHAR(255) = ''dbo''
AS

/* Stored Procedure Wei_AddOfacResponseMessage
		
		@RequestId  --
		@ResponseMessage -- 
		@Status -- 
		@IsError --
		@ModifiedOper --
	   
		History:
		Modified Date		Author		Change
	   
*/
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
			
	SET NOCOUNT ON
	SELECT @ErrorMsg = ''Update to ResponseMessage Column on Requests table failed'', @TableName = ''Requests''
	
	BEGIN TRY
		BEGIN TRANSACTION Wei_AddOfacResponseMessage
		
		UPDATE	Requests 
		SET		ResponseMessage=@ResponseMessage, 
				[Status]=@Status, 
				iserror=@IsError,
				ModifiedDatetime=GETDATE(), 
				ModifiedOper=@modifiedoper 
		WHERE	ID=@RequestId
		
		COMMIT TRANSACTION Wei_AddOfacResponseMessage
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_AddOfacResponseMessage
		RAISERROR (@ErrorMsg,16,1)
	END CATCH
' 
END