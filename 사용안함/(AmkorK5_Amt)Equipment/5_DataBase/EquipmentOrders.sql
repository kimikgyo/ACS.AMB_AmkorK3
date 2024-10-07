USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[EquipmentOrders]    Script Date: 2023-11-23 ¿ÀÈÄ 2:32:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EquipmentOrders](
	[Id] [int] NOT NULL,
	[EQP_NAME] [varchar](50) NOT NULL,
	[COMMAND] [varchar](50) NOT NULL,
	[INCH_TYPE] [varchar](50) NOT NULL,
	[IF_FLAG] [varchar](50) NOT NULL,
	[CREATE_DT] [datetime] NOT NULL,
	[MODIFY_DT] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EquipmentOrders] ADD  DEFAULT (getdate()) FOR [CREATE_DT]
GO

ALTER TABLE [dbo].[EquipmentOrders] ADD  DEFAULT (getdate()) FOR [MODIFY_DT]
GO


