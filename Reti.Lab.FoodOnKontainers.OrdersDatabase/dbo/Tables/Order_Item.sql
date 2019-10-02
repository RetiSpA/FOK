CREATE TABLE [dbo].[Order_Item] (
    [Id_Order]     INT             NOT NULL,
    [Id_Menu_Item] INT             NOT NULL,
    [Menu_Item_Name]    NVARCHAR (50)   NOT NULL,
    [Price]        DECIMAL (18, 2) NOT NULL,
    [Quantity]     INT             NOT NULL,
    CONSTRAINT [PK_Order_Item] PRIMARY KEY CLUSTERED ([Id_Order] ASC, [Id_Menu_Item] ASC),
    CONSTRAINT [FK_Order_Item_Order] FOREIGN KEY ([Id_Order]) REFERENCES [dbo].[Order] ([Id])
);

