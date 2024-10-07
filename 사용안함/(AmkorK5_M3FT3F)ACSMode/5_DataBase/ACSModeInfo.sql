USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[ACSModeInfo]    Script Date: 2023-11-23 ¿ÀÀü 11:30:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACSModeInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Location] [varchar](50) NULL,
	[ACSMode] [varchar](50) NULL,
	[ElevatorMode] [varchar](50) NULL,
 CONSTRAINT [PK_ElevatorMode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


