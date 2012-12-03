
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_AddAudit]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Wei_AddAudit]
(
	@RequestId   INT,
	@AuditLevel	 INT,
	@Message	 NVARCHAR(MAX),
	@CreatedOper VARCHAR(255) = ''dbo''
)
AS
/* Stored Procedure Wei_AddAudit
		
		@RequestId --
		@AuditLevel --
		@Message --
		@CreatedOper --
	   
		History:
	   Modified Date		Author		Change
	   
*/
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
	
	SET NOCOUNT ON
	
	SELECT @ErrorMsg = ''Update to ProcessLock column on Requests table failed'', @TableName = ''Requests''
	
	BEGIN TRY
		BEGIN TRANSACTION Wei_AddAudit
		DECLARE @Status INT
		SELECT @Status=[Status] FROM Requests WHERE id=@requestId

		INSERT INTO [Audit] 
		(
			RequestId, 
			[Status],
			[Level], 
			[Message], 
			CreatedOper, 
			CreatedDatetime
		)
		VALUES 
		(
			@RequestId, 
			@Status, 
			@AuditLevel, 
			@Message, 
			@CreatedOper, 
			GETDATE()
		)
		COMMIT TRANSACTION Wei_AddAudit
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_AddAudit
		RAISERROR (@ErrorMsg,16,1)
	END CATCH
	RETURN 0

' 
END