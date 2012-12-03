IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Interfaces]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Interfaces](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Handler] [int] NOT NULL,
	[Driver] [int] NOT NULL,
	[FileFormat] [nvarchar](100) NOT NULL,
	[Config] [nvarchar](max) NULL,
 CONSTRAINT [PK_Interfaces_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Interfaces_Handler]') AND parent_object_id = OBJECT_ID(N'[dbo].[Interfaces]'))
ALTER TABLE [dbo].[Interfaces]  WITH CHECK ADD  CONSTRAINT [FK_Interfaces_Handler] FOREIGN KEY([Handler])
REFERENCES [dbo].[Handlers] ([Id])

ALTER TABLE [dbo].[Interfaces] CHECK CONSTRAINT [FK_Interfaces_Handler]