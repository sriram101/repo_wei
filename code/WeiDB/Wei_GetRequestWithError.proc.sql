
GO
/****** Object:  StoredProcedure [dbo].[Wei_GetRequestWithError]    Script Date: 06/03/2012 05:55:19 ******/
CREATE PROCEDURE [dbo].[Wei_GetRequestWithError]  
(
	@InterfaceId INT  
)
AS  
BEGIN
SET NOCOUNT ON  
  
/*  
 Procedure Name	:	Wei_GetRequestWithError  
 Author			:	Rama Pappu  
 Create Date	:	4/15/2012  
 Description	: The Stored Procedure will return all the error messages information for a given interface. 
				  The stored procedures accepts the following  
 Parameters:  
 @InterfaceId - InterfaceId  
 History		:  
 Modified Date  Author  Change  
  
*/  

 SELECT  ID,InterfaceID, [Name],[Status], IsError, RequestHeaders, 
		 MessageBody,OfacStatus,	CASE WHEN HasCTC = 1 THEN 'Yes' ELSE 'No' END 'HasCTC',
		 CAST(CreatedDatetime AS VARCHAR) CreatedDatetime,
		 CreatedOper,ModifiedDatetime, ModifiedOper
 FROM	 Requests 
 WHERE   InterfaceId = @InterfaceId  
 AND	 IsError=1   
END;