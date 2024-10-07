USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[ErrorCodeList]    Script Date: 2023-12-15 ¿ÀÀü 8:40:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ErrorCodeList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorCode] [int] NOT NULL,
	[ErrorMessage] [varchar](500) NULL,
	[ErrorType] [varchar](50) NULL,
	[Explanation] [varchar](500) NULL
) ON [PRIMARY]
GO


