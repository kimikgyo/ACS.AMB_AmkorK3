USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[MonitorPcList]    Script Date: 2023-11-23 ¿ÀÀü 11:40:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MonitorPcList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IpAddress] [varchar](50) NULL,
	[ZoneName] [varchar](50) NULL,
	[BcrExist] [int] NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO


