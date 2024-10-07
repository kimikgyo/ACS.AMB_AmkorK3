USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[WiseTowerLampConfigs]    Script Date: 2023-11-23 ¿ÀÀü 11:43:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WiseTowerLampConfigs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameSetting] [varchar](50) NULL,
	[PositionZoneSetting] [varchar](50) NULL,
	[ControlSetting] [varchar](50) NULL,
	[TowerLampUseSetting] [varchar](50) NULL,
	[IpAddressSetting] [varchar](50) NULL,
	[DisplayNameSetting] [varchar](50) NULL,
	[OperationtimeSetting] [int] NULL,
	[ProductValueSetting] [int] NULL,
	[ProductActiveSetting] [int] NULL,
	[ProductName] [varchar](50) NULL,
	[Register22ValueSetting] [varchar](50) NULL,
	[Register32ValueSetting] [varchar](50) NULL,
	[Register33ValueSetting] [varchar](50) NULL,
	[Register34ValueSetting] [varchar](50) NULL,
	[Register35ValueSetting] [varchar](50) NULL,
	[DisplayFlag] [int] NULL,
 CONSTRAINT [PK_EtnLampConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


