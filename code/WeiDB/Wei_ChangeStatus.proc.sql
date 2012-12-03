
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_ChangeStatus]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_ChangeStatus]
(
	@RequestId		INT, 
	@Status			INT,
	@IsError		INT,
	@OfacStatus		INT,
	@ModifiedOper	VARCHAR(255) = ''dbo''
)
AS
/* Stored Procedure Wei_ChangeStatus
		
		@RequestId --
		@Status --
		@IsError --
		@OfacStatus --
		@ModifiedOper --
	   
		History:
	   Modified Date		Author		Change
	   
*/
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
	--- ********************* Begin Rule Procedure **********************************
	SET NOCOUNT ON
	
	SELECT @ErrorMsg = ''Update to Status column on Requests table failed'', @TableName = ''Requests''
	BEGIN TRY
		BEGIN TRAN Wei_ChangeStatus
	
		UPDATE	Requests 
		SET		[Status] =@Status, 
				IsError=@IsError, 
				OfacStatus=@OfacStatus,
				ModifiedDatetime=GETDATE(), 
				ModifiedOper=@ModifiedOper 
		WHERE	Id=@Requestid
		
		COMMIT TRANSACTION Wei_ChangeStatus
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_ChangeStatus
		RAISERROR (@ErrorMsg,16,1)
	END CATCH
	RETURN 0
' 
END