
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_AddOfacResponse]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_AddOfacResponse]
(
	@RequestId		INT, 
	@ResponseBody	NVARCHAR(MAX),
	@Identifier		VARCHAR(255),
	@CreatedOper	VARCHAR(255) = ''dbo''
)
AS
/* Stored Procedure Wei_AddOfacResponse
		
		@RequestId		-- 
		@ResponseBody	--
		@Identifier		--
		@CreatedOper	--
	   
		History:
	   Modified Date		Author		Change
	   
*/
	DECLARE	@Err		INT,
			@TableName	SYSNAME,
			@ErrorMsg	NVARCHAR(2000)
			
	SET NOCOUNT ON
	SELECT @ErrorMsg = ''Insert into OFACResponse table failed'', @TableName = ''OFACResponse''
	
	BEGIN TRY
		BEGIN TRANSACTION Wei_AddOfacResponse
		INSERT INTO OfacResponses 
		(
			RequestId, 
			ResponseBody , 
			Identifier, 
			CreatedDatetime , 
			CreatedOper
		)
		VALUES 
		(
			@RequestId, 
			@ResponseBody, 
			@Identifier, 
			GETDATE(), 
			@CreatedOper
		)
		COMMIT TRANSACTION Wei_AddOfacResponse
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION Wei_AddOfacResponse
		RAISERROR (@ErrorMsg,16,1)
	END CATCH
	RETURN 0

' 
END