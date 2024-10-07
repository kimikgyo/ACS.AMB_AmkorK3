USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[CallButtons]    Script Date: 2023-11-23 ¿ÀÀü 11:34:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CallButtons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ButtonIndex] [int] NULL,
	[ButtonName] [varchar](50) NULL,
	[IpAddress] [varchar](50) NULL,
	[ConnectionState] [varchar](50) NULL,
	[LastAccessTime] [datetime] NULL,
	[AccessElapsedTime] [float] NULL,
	[MissionCount] [int] NULL,
	[MissionStateText] [varchar](50) NULL,
 CONSTRAINT [PK_CallButtons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


