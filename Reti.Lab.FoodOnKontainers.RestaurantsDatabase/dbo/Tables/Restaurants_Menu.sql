CREATE TABLE [dbo].[Restaurants_Menu] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Id_Restaurant] INT             NOT NULL,
    [Name]          NVARCHAR (50)   NOT NULL,
    [Description]   NVARCHAR (MAX)  NOT NULL,
    [Id_Dish_Type]  INT             NOT NULL,
    [Price]         DECIMAL (18, 2) NULL,
    [Promo]         DECIMAL (3, 2)  NULL,
    CONSTRAINT [PK_Restaurants_Menu] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Restaurants_Menu_Dish_Types] FOREIGN KEY ([Id_Dish_Type]) REFERENCES [dbo].[Dish_Types] ([Id]),
    CONSTRAINT [FK_Restaurants_Menu_Restaurants] FOREIGN KEY ([Id_Restaurant]) REFERENCES [dbo].[Restaurants] ([Id])
);

