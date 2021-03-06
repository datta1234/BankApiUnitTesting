USE [Bank]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2/9/2021 11:01:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[AccountBalance] [decimal](10, 2) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2/9/2021 11:01:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[DateCreated] [datetime] NULL,
	[DateUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialTransaction]    Script Date: 2/9/2021 11:01:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[FinancialAccountId] [int] NULL,
	[TransactionType] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[RunningTotal] [decimal](10, 2) NULL,
	[Description] [nvarchar](200) NULL,
	[DateCreated] [datetime] NULL,
	[FromAccount] [int] NULL,
	[ToAccount] [int] NULL
) ON [PRIMARY]
GO
