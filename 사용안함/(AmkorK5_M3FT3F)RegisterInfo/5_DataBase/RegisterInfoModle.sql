USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[RegisterInfoModle]    Script Date: 2023-11-23 ¿ÀÈÄ 1:32:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegisterInfoModle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ACSRobotGroup] [varchar](500) NULL,
	[RegisterNumber] [int] NULL,
	[RegisterValue] [int] NULL,
	[RegisterInfoMessge] [varchar](5000) NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO


