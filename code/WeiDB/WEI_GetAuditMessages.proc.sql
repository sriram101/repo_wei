
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wei_GetAuditMessages]') 
	AND type in (N'P', N'PC'))
BEGIN
	CREATE PROCEDURE [dbo].[Wei_GetAuditMessages]
	(
		@RequestId INT
	)
AS

SET NOCOUNT ON
	
	/*  
		 Stored Procedure Name	:	[Wei_GetAuditMessages]  
		 Author					:	Rama Pappu  
		 Create Date			:	4/30/2012  
		 Description: The Stored Procedure will return the audit information with current status  
		 for a given requesr. The stored procedures accepts the following  
		 parameters:  
		 @RequestID - Message Request Identified
		 History:  
		 Modified Date  Author  Change  
		 
  
*/  


	SELECT	ID, level, [Status], CreatedDatetime
	FROM	Audit where  requestId = @requestId
	ORDER	BY CreatedDatetime

END
