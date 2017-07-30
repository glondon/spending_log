CREATE TABLE [dbo].[personal]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [cost] DECIMAL(18, 2) NOT NULL, 
    [category] VARCHAR(50) NOT NULL, 
    [payment_type] VARCHAR(50) NOT NULL, 
    [date] DATE NOT NULL
)
