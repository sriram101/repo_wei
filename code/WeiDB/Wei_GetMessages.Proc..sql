CREATE PROCEDURE [dbo].[Wei_GetMessages]    
(    
 @StartDate			DATETIME,    
 @EndDate			DATETIME,    
 @Status			NVARCHAR(50),  
 @HasCTC			BIT,  
 @IsError			BIT,
 @SearchText		NVARCHAR(4000),
 @Sorting			NVARCHAR(1000)=NULL,      
 @CurrentPage		INT,
 @PageSize			INT,
 @TotalRecords		INT OUTPUT
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
 Modified Date  Author  Change  
 01/09/20111   Rama Pappu Added Interface Name column to the Query to be displayed on user interface  
         Added OFAC Status column values to the Query to be displayed on user interface  
         When ModifiedDateTime column is null, the value will be null. Earlier the   
         CreatedDatetime value was assigned when ModifiedDateTime is null.  
 02/09/2011   Rama Pappu Added a new parameter hasCTC to identify if a message has CTC codes or not  
 02/13/2011   Rama Pappu Added a new parameter IsError to filter messages that have errored out  
  
*/  
	DECLARE @Start  INT
	DECLARE @FirstRow INT, @LastRow INT      
      
	IF @CurrentPage = 0
		SET @CurrentPage = 1
  
	IF @PageSize IS NULL      
		SET @PageSize = 10;

	SET @FirstRow = (@CurrentPage - 1)*@PageSize + 1;      
	SET @LastRow = @FirstRow + @PageSize -1;

	SET @StartDate = @StartDate   
	SET @EndDate = @EndDate + ' 23:59:59.997'  
    
	IF @HasCTC = 0  
		SET @HasCTC = NULL  
  
	IF @IsError = 0  
		SET @IsError = NULL   


	IF @Sorting IS NULL      
		SET @Sorting = 'ModifiedDateTime DESC';

	SET @CurrentPage = ABS(@CurrentPage)

	SET @Start = ABS(@PageSize * (@CurrentPage-1))
	
	CREATE TABLE #Messages(
		SlNo				INT  NOT NULL,
		ID					INT,
		Name				NVARCHAR(255),
		InterfaceName		NVARCHAR(255),
		CreatedDateTime		DATETIME,
		ModifiedDateTime	DATETIME,
		OFACStatus			NVARCHAR(255),
		Description			NVARCHAR(100),
		IsError				VARCHAR(10),
		MessageBody			NTEXT,
		TranslatedMessage	NTEXT,
		ResponseMessage		NTEXT
	)

	DECLARE @sql NVARCHAR(MAX)      
        
    SELECT @sql = '      
		INSERT INTO #Messages
		(
			ID,
			Name,
			InterfaceName,
			CreatedDateTime,		
			OFACStatus,	
			[Description],	
			IsError,	
			MessageBody,
			TranslatedMessage,
			ResponseMessage,
			ModifiedDateTime

		)
		SELECT	TOP ('+ CAST(@LastRow AS VARCHAR) + ')
				ROW_NUMBER() OVER (ORDER BY ' + @Sorting + ') AS RowNumber,
				R.ID, R.[name], I.Name InterfaceName, CreatedDateTime,   
				CASE WHEN OFACStatus = 1 THEN   
					''Unprocessed''   
				WHEN OfacStatus = 2 THEN  
					''OK''  
				WHEN OfacStatus = 3 THEN    
					''Confirmed''
				END OFACStatus,    
				[Description],  
				CASE WHEN IsError = 1 THEN ''Yes'' ELSE ''No'' END IsError,  
				MessageBody,TranslatedMessage,ResponseMessage,   
				CASE WHEN ModifiedDatetime IS NULL THEN NULL ELSE ModifiedDatetime END ModifiedDatetime    
		FROM	Requests R    
		INNER	JOIN [Status] S ON S.ID = R.[Status]    
		INNER	JOIN Interfaces I ON I.ID = R.InterfaceID  
		WHERE	CreatedDatetime BETWEEN '''+@StartDate+''' AND '''+@EndDate+'''
		AND		([Status] = @Status OR @Status IS NULL OR @Status = '''')    
		AND		(HasCTC = @HasCTC OR @HasCTC IS NULL)  
		AND		(IsError = @IsError OR @IsError IS NULL)  
		AND		(((CHARINDEX(@SearchText, messagebody ) > 0) OR @SearchText IS NULL OR @SearchText = '''') 
		OR		((CHARINDEX(@SearchText,translatedmessage)> 0) OR @SearchText IS NULL OR @SearchText = '''')
		OR		((CHARINDEX(@SearchText,responsemessage) > 0) OR @SearchText IS NULL OR @SearchText = ''''))


SET ROWCOUNT @PageSize

SELECT @TotalRecords = COUNT(*) FROM #Messages


SELECT	ID,Name, InterfaceName,CreatedDateTime,	OFACStatus,	[Description],IsError,MessageBody,TranslatedMessage,ResponseMessage,ModifiedDateTime
FROM	#Messages WHERE SlNo > @Start

SET ROWCOUNT 0


END
