USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 2023-11-23 ¿ÀÈÄ 1:13:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateTime] [datetime] NULL,
	[Barcode] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[ProductName] [varchar](50) NULL,
	[Qty] [int] NULL,
	[Info1] [varchar](50) NULL,
	[Info2] [varchar](50) NULL,
	[Info3] [varchar](50) NULL,
	[Info4] [varchar](50) NULL
) ON [PRIMARY]
GO


