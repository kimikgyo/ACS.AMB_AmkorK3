USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[ElevatorState]    Script Date: 2023-11-23 ¿ÀÀü 11:20:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ElevatorState](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RobotName] [varchar](50) NULL,
	[MirStateElevator] [varchar](50) NULL,
	[ElevatorState] [varchar](50) NULL,
	[ElevatorMissionName] [varchar](50) NULL,
 CONSTRAINT [PK_ElevatorState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


