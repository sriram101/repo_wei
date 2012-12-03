
GO
/****** Object:  StoredProcedure [dbo].[Wei_GetAuditMessagesByRequest]    Script Date: 06/03/2012 16:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_GetAuditMessagesByRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Wei_GetAuditMessagesByRequest]
(
	@RequestID INT
)
AS
/*
	Stored Procedure Name: Wei_GetAuditMessagesByRequest
	Author:Rama Pappu
	Create Date: 01/04/2010
	Description: The Stored Procedure will return all the audit details for a particular request.
	parameters:
	@RequestID - Request ID associated with a message
	History:
	Modified Date		Author		Change

*/

SELECT A.ID, [Description], 
		CASE WHEN [level] = 0 THEN 
			''Debug'' 
			WHEN [level] = 1 THEN 
			''Information''
			WHEN [level]=2 THEN 
			''Error''
		END [Level],
		CreatedDateTime, [message]
FROM Audit A
INNER JOIN Status S ON S.ID =A.Status
WHERE RequestID = @RequestID
ORDER BY CreatedDateTime DESC


' 
END