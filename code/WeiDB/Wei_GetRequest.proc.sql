CREATE PROCEDURE [dbo].[Wei_GetRequest]
	@RequestId	INT
AS

/* Stored Procedure Wei_GetRequest
		
		@RequestId  --
			   
		History:
		Modified Date		Author		Change
	   
*/
		SELECT	[Name],
				[InterfaceID],
				[RequestHeaders],
				[MessageBody],
				[Status],
				[IsError],
				[HasCTC],
				[OFACStatus],
				[ProcessLock],
				[CreatedDatetime],
				[CreatedOper]
		FROM	Requests WHERE Id = @RequestId