CREATE TABLE [dbo].[SearchProductCategory](
	[ProductId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[CategoryId] ASC
)
)

