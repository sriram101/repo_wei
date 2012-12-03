
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_AddRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_AddRequest]
(
	@Name			NVARCHAR(255),
	@InterfaceId	INT,
	@RequestHeader	NVARCHAR(MAX),
	@MessageBody	NVARCHAR(MAX),
	@Status			INT,
	@IsError		INT,
	@CreatedOper	VARCHAR(255) = ''dbo''
)
AS

	/* Stored Procedure Add_Request
		
	   @Name 			-- 
	   @InterfaceID	    --  
	   @RequestHeadr   -- 
	   @MessageBody		-- 
	   @Status		    -- 
	   @IsError         -- 
	   @CreatedOper		-- 
	   
		History:
	   Modified Date		Author		Change
	   
	  */

	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
			
	--- ********************* Begin Rule Procedure **********************************
	SET NOCOUNT ON
	

	BEGIN TRY
		BEGIN TRANSACTION Wei_AddRequest
	
		SELECT @ErrorMsg = ''Insert into Requests table failed'', @TableName = ''Requests''
		INSERT INTO [Requests]
           (
				[Name],
				[InterfaceID],
				[RequestHeaders],
				[MessageBody],
				[Status],
				[IsError],
				[HasCTC],
				[OFACStatus],
				[ProcessLock],
				[CreatedDatetime],
				[CreatedOper])
		VALUES 
			(
				@Name,
				@InterfaceId,
				@RequestHeader,
				@MessageBody,
				@Status,
				@IsError,
				0,
				1,
				0,
				GETDATE(),
				''dbo''
			)
		COMMIT TRANSACTION Wei_AddRequest
		SELECT @@IDENTITY	
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_AddRequest
		RAISERROR (@ErrorMsg,16,1)
		
	END CATCH
	--RETURN @@IDENTITY	
 
' 
END