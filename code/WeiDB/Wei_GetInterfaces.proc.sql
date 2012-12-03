
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_GetInterfaces]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Wei_GetInterfaces]
	
AS
/* Stored Procedure Wei_GetInterfaces
	   
	   History:
	   Modified Date		Author		Change
	   
*/
	SET NOCOUNT ON
	SELECT	I.ID, I.Name, I.Handler, I.FileFormat,
			I.Driver, I.Config,
			D.Name  DriverName, 
			D.Dll DriverDll, D.Type DriverType, 
			H.Name HandlerName, H.Dll HandlerDll, 
			H.Type HandlerType 
	FROM	Interfaces I 
	INNER	JOIN Handlers H ON I.Handler = H.ID
	INNER	JOIN Drivers D ON I.Driver=D.Id 

	RETURN 0
' 
END