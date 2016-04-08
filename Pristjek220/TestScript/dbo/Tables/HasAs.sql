CREATE TABLE [dbo].[HasAs] (
    [StoreId]   INT        NOT NULL,
    [ProductId] INT        NOT NULL,
    [Price]     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_dbo.HasAs] PRIMARY KEY CLUSTERED ([StoreId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_dbo.HasAs_dbo.Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.HasAs_dbo.Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[Stores] ([StoreId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_StoreId]
    ON [dbo].[HasAs]([StoreId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[HasAs]([ProductId] ASC);

