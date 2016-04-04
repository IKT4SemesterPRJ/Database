CREATE TABLE [dbo].[Stores] (
    [StoreId]   INT            IDENTITY (1, 1) NOT NULL,
    [StoreName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Stores] PRIMARY KEY CLUSTERED ([StoreId] ASC)
);

