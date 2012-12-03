
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Requests]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[RequestHeaders] [nvarchar](max) NULL,
	[InterfaceId] [int] NOT NULL,
	[MessageBody] [ntext] NOT NULL,
	[Status] [int] NOT NULL,
	[IsError] [bit] NOT NULL CONSTRAINT [DF_Requests_iserror]  DEFAULT ((0)),
	[OFACStatus] [int] NULL,
	[HasCTC] [bit] NOT NULL CONSTRAINT [DF_Requests_hasctc]  DEFAULT ((0)),
	[ProcessLock] [int] NULL,
	[TranslatedMessage] [ntext] NULL,
	[ResponseMessage] [ntext] NULL,
	[CreatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_Requests_CreatedDateTime]  DEFAULT (getdate()),
	[CreatedOper] [nvarchar](50) NOT NULL,
	[ModifiedDateTime] [datetime] NULL,
	[ModifiedOper] [nvarchar](50) NULL,
 CONSTRAINT [PK_Requests__Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Requests_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[Requests]'))
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_ID] FOREIGN KEY([Id])
REFERENCES [dbo].[Requests] ([Id])

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_ID]