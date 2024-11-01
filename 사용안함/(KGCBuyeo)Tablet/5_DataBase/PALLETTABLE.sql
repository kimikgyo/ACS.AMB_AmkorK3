USE [Aicellomilim_Iksan]
GO

/****** Object:  Table [dbo].[PALLETTABLE]    Script Date: 2023-11-23 ���� 2:19:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PALLETTABLE](
	[SEQ] [int] IDENTITY(1,1) NOT NULL,
	[ZONENAME] [varchar](50) NOT NULL,
	[PALLETNO] [int] NOT NULL,
	[REGDATE] [datetime] NOT NULL,
	[DisplayFlag] [int] NOT NULL,
 CONSTRAINT [PK__PALLETTA__CA1938C0EF1DDF1B] PRIMARY KEY CLUSTERED 
(
	[SEQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


