USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[PartModels]    Script Date: 2023-11-23 ¿ÀÈÄ 2:44:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PartModels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LINE_CD] [varchar](50) NULL,
	[POST_CD] [int] NULL,
	[COMM_PO] [varchar](50) NULL,
	[OUT_Q] [int] NULL,
	[COMM_ANG] [int] NULL,
	[PART_CD] [varchar](50) NULL,
	[PART_NM] [varchar](50) NULL,
	[NP_MODE] [varchar](50) NULL,
	[NP_OUT_Q] [int] NULL,
	[NP_PART_CD] [varchar](50) NULL,
	[NP_PART_NM] [varchar](50) NULL,
 CONSTRAINT [PK_PartModelsTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


