USE [Wei]
GO

/****** Object:  Table [dbo].[Status]    Script Date: 01/12/2011 11:02:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_TYPE='BASE TABLE' 
    AND TABLE_NAME='Status')
    DROP TABLE [Status]
    
GO
    
CREATE TABLE [dbo].[Status](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (SELECT * FROM Status WHERE [Description] = 'UnProcessed')
BEGIN
	INSERT INTO Status([Description])
	VALUES ('UnProcessed')
END
	
	GO
IF NOT EXISTS (SELECT * FROM Status WHERE [Description] = 'Translated')
BEGIN
	INSERT INTO Status([Description])
	VALUES ('Translated')
END
GO
IF NOT EXISTS (SELECT * FROM Status WHERE [Description] = 'SentForOFACCheck')
BEGIN
	INSERT INTO Status([Description])
	VALUES ('SentForOFACCheck')
END
GO
IF NOT EXISTS (SELECT * FROM Status WHERE [Description] = 'OFACResponseReceived')
BEGIN
	INSERT INTO Status([Description])
	VALUES ('OFACResponseReceived')
END
GO
IF NOT EXISTS (SELECT * FROM Status WHERE [Description] = 'Processed')
BEGIN
	INSERT INTO Status([Description])
	VALUES ('Processed')
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT name FROM sysobjects 
         WHERE name = 'Wei_GetMessages' AND type = 'P')
   DROP PROCEDURE Wei_GetMessages
GO

CREATE PROCEDURE [dbo].[Wei_GetMessages]  
(  
 @StartDate DATETIME,  
 @EndDate	DATETIME,  
 @Status	NVARCHAR(50),
 @HasCTC	BIT,
 @IsError	BIT
)  
  
AS  

SET NOCOUNT ON

/*
	Stored Procedure Name: Wei_GetMessages
	Author:Rama Pappu
	Create Date: 12/31/2010
	Description: The Stored Procedure will return the message information with current status
	for the current date or between range of dates. The stored procedures accepts the following
	parameters:
	@startDate - Message Create Date
	@EndDate - Message Create Date
	@Status - Message Status
	History:
	Modified Date		Author		Change
	01/09/20111			Rama Pappu	Added Interface Name column to the Query to be displayed on user interface
									Added OFAC Status column values to the Query to be displayed on user interface
									When ModifiedDateTime column is null, the value will be null. Earlier the 
									CreatedDatetime value was assigned when ModifiedDateTime is null.
	02/09/2011			Rama Pappu	Added a new parameter hasCTC to identify if a message has CTC codes or not
	02/13/2011			Rama Pappu	Added a new parameter IsError to filter messages that have errored out

*/
  
  SET @StartDate = @StartDate 
  SET @EndDate = @EndDate + ' 23:59:59.997'
  
  IF @HasCTC = 0
	SET @HasCTC = NULL

  IF @IsError = 0
	SET @IsError = NULL	
  
SELECT 
      R.ID, R.[name], I.Name InterfaceName, CreatedDateTime, 
	CASE WHEN OFACStatus = 1 THEN 
		'Unprocessed' 
	WHEN OfacStatus = 2 THEN
		'OK'
	WHEN OfacStatus = 3 THEN  
		'Confirmed' 
	END OFACStatus,  
[Description],  CASE WHEN IsError = 1 THEN 'Yes' ELSE 'No' END IsError,
MessageBody,TranslatedMessage,ResponseMessage, 
CASE WHEN ModifiedDatetime IS NULL THEN NULL ELSE ModifiedDatetime END ModifiedDatetime  
FROM Requests R  
INNER JOIN [Status] S ON S.ID = R.[Status]  
INNER JOIN Interfaces I ON I.ID = R.InterfaceID
WHERE CreatedDatetime BETWEEN @StartDate AND @EndDate  
AND ([Status] = @Status OR @Status IS NULL OR @Status = '')  
AND (HasCTC = @HasCTC OR @HasCTC IS NULL)
AND	(IsError = @IsError OR @IsError IS NULL)
  


GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects 
         WHERE name = 'Wei_GetMessageStatus' AND type = 'P')
   DROP PROCEDURE Wei_GetMessageStatus
GO
CREATE PROCEDURE [dbo].[Wei_GetMessageStatus]
AS

SELECT ID, [Description]
FROM Status

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sysobjects 
         WHERE name = 'Wei_GetAuditMessagesByRequest' AND type = 'P')
   DROP PROCEDURE Wei_GetAuditMessagesByRequest
GO
CREATE PROCEDURE [dbo].[Wei_GetAuditMessagesByRequest]
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
			'Debug' 
			WHEN [level] = 1 THEN 
			'Information'
			WHEN [level]=2 THEN 
			'Error'
		END [Level],
		CreatedDateTime, [message]
FROM Audit A
INNER JOIN Status S ON S.ID =A.Status
WHERE RequestID = @RequestID
ORDER BY CreatedDateTime DESC

GO
