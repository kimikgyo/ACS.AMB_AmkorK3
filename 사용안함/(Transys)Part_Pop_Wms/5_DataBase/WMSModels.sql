USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[WMSModels]    Script Date: 2023-11-23 ¿ÀÈÄ 2:42:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WMSModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LINE_CD] [varchar](50) NULL,
	[POST_CD] [int] NULL,
	[COMM_ANG] [int] NULL,
	[RETU_TYPE] [varchar](50) NULL,
	[OUT_Q] [int] NULL,
	[PART_CD] [varchar](50) NULL,
	[PART_NM] [varchar](50) NULL,
	[OUT_WH] [varchar](50) NULL,
	[OUT_POINT] [int] NULL,
	[WMS_IF_FLAG] [varchar](50) NULL,
	[CREATE_DT] [datetime] NULL,
	[MODIFY_DT] [datetime] NULL,
 CONSTRAINT [PK_WMSModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


