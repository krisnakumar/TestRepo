-------------****Scripts to create Query table to store the user queries****-------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Query](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QueryId] [varchar](150) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[QueryJson] [varchar](max) NOT NULL,
	[QuerySQL] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-------------****Scripts to create User Query table to store the user queries****-------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserQuery](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QueryId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Role] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserQuery]  WITH CHECK ADD  CONSTRAINT [FK_Company_Query] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserQuery] CHECK CONSTRAINT [FK_Company_Query]
GO

ALTER TABLE [dbo].[UserQuery]  WITH CHECK ADD  CONSTRAINT [FK_Role] FOREIGN KEY([Role])
REFERENCES [dbo].[Role] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserQuery] CHECK CONSTRAINT [FK_Role]
GO

ALTER TABLE [dbo].[UserQuery]  WITH CHECK ADD  CONSTRAINT [FK_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserQuery] CHECK CONSTRAINT [FK_User]
GO
