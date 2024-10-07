USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[EquipmentCallNames]    Script Date: 2023-11-23 ¿ÀÈÄ 2:32:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EquipmentCallNames](
	[GROUP_NAME] [varchar](50) NOT NULL,
	[EQP_NAME] [varchar](50) NOT NULL,
	[INCH_TYPE] [varchar](50) NOT NULL,
	[CALL_NAME] [varchar](50) NOT NULL
) ON [PRIMARY]
GO


