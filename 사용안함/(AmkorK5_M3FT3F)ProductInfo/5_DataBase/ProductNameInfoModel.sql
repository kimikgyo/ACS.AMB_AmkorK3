USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[ProductNameInfoModel]    Script Date: 2023-11-23 ¿ÀÈÄ 1:13:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductNameInfoModel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Regiser22Vlaue] [int] NULL,
	[RegisterNo] [varchar](50) NULL,
	[RegisterValue] [int] NULL,
	[ProductName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO


