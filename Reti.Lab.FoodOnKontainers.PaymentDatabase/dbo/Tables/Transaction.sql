USE [Fok_Payment]
GO

/****** Object:  Table [dbo].[Transaction]    Script Date: 24/05/2019 14:54:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[Total] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[PaySystem] [int] NOT NULL,
	[Status] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


