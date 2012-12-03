/****** Object:  Table [dbo].[Status]    Script Date: 02/06/2011 04:11:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Status')
BEGIN    
	CREATE TABLE [dbo].[Status](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](100) NULL,
	 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Drivers')
BEGIN    
	CREATE TABLE [dbo].[Drivers](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](255) NOT NULL,
		[dll] [varchar](255) NOT NULL,
		[type] [varchar](255) NULL,
		[config] [text] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Handlers')
BEGIN    
	CREATE TABLE [dbo].[Handlers](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](255) NOT NULL,
		[dll] [varchar](255) NOT NULL,
		[type] [varchar](255) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Interfaces')
BEGIN        
	CREATE TABLE [dbo].[Interfaces](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](255) NOT NULL,
		[handler] [int] NOT NULL,
		[driver] [int] NOT NULL,
		[fileformat] [varchar](100) NOT NULL,
		[config] [text] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Interfaces]  ADD  CONSTRAINT [FK_Interfaces_Driver] FOREIGN KEY([driver])
	REFERENCES [dbo].[Drivers] ([id])

	ALTER TABLE [dbo].[Interfaces]  ADD  CONSTRAINT [FK_Interfaces_Handler] FOREIGN KEY([handler])
	REFERENCES [dbo].[Handlers] ([id])

END
GO

IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Requests')
BEGIN
    
	CREATE TABLE [dbo].[Requests](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](255) NOT NULL,
		[requestheaders] [ntext] NULL,
		[interfaceid] [int] NOT NULL,
		[messagebody] [ntext] NOT NULL,
		[status] [int] NOT NULL,
		[ofacstatus] [int] NULL,
		[processlock] [int] NULL,
		[translatedmessage] [ntext] NULL,
		[responsemessage] [ntext] NULL,
		[createddatetime] [datetime] NOT NULL,
		[createdoper] [varchar](255) NOT NULL,
		[modifieddatetime] [datetime] NULL,
		[modifiedoper] [varchar](255) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Requests]  ADD  CONSTRAINT [FK_Requests_Status] FOREIGN KEY([status])
	REFERENCES [dbo].[Status] ([ID])
	
	
END
GO


IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='OFACResponses')
BEGIN

	CREATE TABLE [dbo].[OfacResponses](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[requestid] [int] NOT NULL,
		[responsebody] [ntext] NULL,
		[identifier] [varchar](255) NOT NULL,
		[createddatetime] [datetime] NOT NULL,
		[createdoper] [varchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	
	ALTER TABLE [dbo].[OfacResponses] ADD  CONSTRAINT [FK_OfacResponses_RequestID] FOREIGN KEY([requestid])
	REFERENCES [dbo].[Requests] ([id])
	
END
GO

IF NOT EXISTS (SELECT * 
    FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Audit')
  BEGIN
	CREATE TABLE [dbo].[Audit](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[requestid] [int] NOT NULL,
		[level] [int] NOT NULL,
		[status] [int] NOT NULL,
		[message] [ntext] NULL,
		[createddatetime] [datetime] NOT NULL,
		[createdoper] [varchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Audit] ADD  CONSTRAINT [FK_Audit_RequestID] FOREIGN KEY([requestid])
	REFERENCES [dbo].[Requests] ([id])
	
END
GO

