CREATE PROCEDURE [dbo].[TestCleanTable]

AS
	Delete dbo.HasAs
	Delete dbo.Products
	Delete dbo.Stores
RETURN 0
