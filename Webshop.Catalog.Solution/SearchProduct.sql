﻿CREATE TABLE [dbo].[SearchProduct](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[SKU] [nvarchar](50) NOT NULL,
	[Price] [int] NOT NULL,
	[Currency] [nvarchar](3) NOT NULL,
	[Description] [ntext] NULL,	
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
