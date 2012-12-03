IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_GetHandlers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[Wei_GetHandlers]
	
AS
	/* Stored Procedure Wei_GetHandlers
		   
		   History:
		   Modified Date		Author		Change
		   
	*/
	
	SELECT ID,[Name],Dll,[Type] FROM Handlers

RETURN 0
' 
END