USE [lms]
GO

/****** Object:  Table [dbo].[Reporting_DashboardRole]    Script Date: 04-04-2019 18:57:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Reporting_DashboardRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[DashboardID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Reporting_DashboardRole]  WITH CHECK ADD  CONSTRAINT [FK_Dashboard_Id] FOREIGN KEY([DashboardID])
REFERENCES [dbo].[Reporting_Dashboard] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Reporting_DashboardRole] CHECK CONSTRAINT [FK_Dashboard_Id]
GO

ALTER TABLE [dbo].[Reporting_DashboardRole]  WITH CHECK ADD  CONSTRAINT [FK_Dashboard_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Reporting_DashboardRole] CHECK CONSTRAINT [FK_Dashboard_Role]
GO


