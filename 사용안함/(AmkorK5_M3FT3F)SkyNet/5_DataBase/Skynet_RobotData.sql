USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[Skynet_RobotData]    Script Date: 2023-11-23 ¿ÀÈÄ 1:33:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Skynet_RobotData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SkyNetMode] [varchar](50) NULL,
	[Linecode] [varchar](50) NULL,
	[Processcode] [varchar](50) NULL,
	[RobotName] [varchar](50) NULL,
	[RobotState] [varchar](50) NULL,
	[MissionName] [varchar](50) NULL,
	[MissionState] [varchar](50) NULL,
 CONSTRAINT [PK_Skynet_EM_DataSend] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


