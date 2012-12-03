IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_AddTranslatedMessage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_AddTranslatedMessage]
(
	@RequestId			INT, 
	@TranslatedMessage	NVARCHAR(MAX),
	@Status				INT,
	@IsError			INT,
	@HasCTC				INT,
	@ModifiedOper		VARCHAR(255) = ''dbo''
)
AS
	/* Stored Procedure Wei_AddTranslatedMessage
		
		@RequestId --
		@TranslatedMessage --
		@Status --
		@IsError --
		@HasCTC --
		@ModifiedOper --
	   
		History:
		Modified Date		Author		Change
	   
	  */
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
	--- ********************* Begin Rule Procedure **********************************
	SET NOCOUNT ON
	
	SELECT @ErrorMsg = ''Update to Requests table failed'', @TableName = ''Requests''
	BEGIN TRY
	
		BEGIN TRAN Wei_AddTranslatedMessage
	
		UPDATE	Requests 
		SET		TranslatedMessage=@TranslatedMessage, 
				[Status]=@Status, 
				IsError=@IsError,
				HasCTC=@HasCTC,
				ModifiedDatetime=GETDATE(), 
				ModifiedOper=@ModifiedOper 
		WHERE	ID=@RequestId
		COMMIT TRANSACTION Wei_AddTranslatedMessage
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN Wei_AddTranslatedMessage
		RAISERROR (@ErrorMsg,16,1)
	END	CATCH
	RETURN 0
' 
END