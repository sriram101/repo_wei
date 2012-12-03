	
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Audit]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Audit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[CreatedDateTime] [datetime] NOT NULL CONSTRAINT [DF_Audit_CreatedDateTime]  DEFAULT (getdate()),
	[CreatedOper] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Audit_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Audit_RequestId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Audit]'))
ALTER TABLE [dbo].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_Audit_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Requests] ([Id])

ALTER TABLE [dbo].[Audit] CHECK CONSTRAINT [FK_Audit_RequestId]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Audit_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[Audit]'))
ALTER TABLE [dbo].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_Audit_Status] FOREIGN KEY([Id])
REFERENCES [dbo].[Audit] ([Id])

ALTER TABLE [dbo].[Audit] CHECK CONSTRAINT [FK_Audit_Status]
	
	
	
GO




