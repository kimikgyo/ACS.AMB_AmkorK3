USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[IP]    Script Date: 2023-11-23 ���� 2:21:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IP](
	[Seq] [int] IDENTITY(1,1) NOT NULL,
	[IP] [varchar](50) NOT NULL,
	[ZONENAME] [varchar](50) NOT NULL,
	[DisplayFlag] [int] NOT NULL,
 CONSTRAINT [PK__IP__CA1E3C881D580A5C] PRIMARY KEY CLUSTERED 
(
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


