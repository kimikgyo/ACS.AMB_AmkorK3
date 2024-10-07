USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[PlcConfigs]    Script Date: 2023-12-15 ¿ÀÀü 11:24:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlcConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlcModuleUse] [varchar](50) NULL,
	[PlcIpAddress] [varchar](50) NULL,
	[PortNumber] [int] NULL,
	[PlcModuleName] [varchar](50) NULL,
	[PlcMapType] [varchar](50) NULL,
	[ReadFirstMapAddress] [varchar](50) NULL,
	[ReadSecondMapAddress] [varchar](50) NULL,
	[WriteFirstMapAddress] [varchar](50) NULL,
	[WriteSecondMapAddress] [varchar](50) NULL,
	[ControlMode] [varchar](50) NULL,
	[CallNotOverlapCount] [int] NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_PlcConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


