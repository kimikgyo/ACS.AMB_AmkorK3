USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[EquipmentStatus]    Script Date: 2023-11-23 ¿ÀÈÄ 2:32:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EquipmentStatus](
	[Id] [int] NOT NULL,
	[EQP_NAME] [varchar](50) NOT NULL,
	[EQP_MODE] [int] NOT NULL,
	[PORT_ACCESS] [int] NOT NULL,
	[PORT_STATUS] [int] NOT NULL,
	[MODIFY_DT] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EquipmentStatus] ADD  DEFAULT (getdate()) FOR [MODIFY_DT]
GO


