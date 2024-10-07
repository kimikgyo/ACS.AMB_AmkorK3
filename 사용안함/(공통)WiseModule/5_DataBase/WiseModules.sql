USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[WiseModules]    Script Date: 2023-11-23 ¿ÀÈÄ 1:58:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WiseModules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleUse] [varchar](50) NULL,
	[ModuleIpAddress] [varchar](50) NULL,
	[ModuleName] [varchar](50) NULL,
	[ModuleStatus] [varchar](50) NULL,
	[ModuleControlMode] [varchar](50) NULL,
	[ModuleIn_Value] [int] NULL,
	[ModuleOut_Value] [int] NULL,
	[DisplayName] [varchar](50) NULL,
	[DisplayFlag] [int] NULL
) ON [PRIMARY]
GO


