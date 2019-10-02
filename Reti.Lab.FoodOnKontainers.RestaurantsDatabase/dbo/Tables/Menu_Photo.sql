CREATE TABLE [dbo].[Menu_Photo] (
    [Id_Menu] INT             NOT NULL,
    [Photo]   VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Menu_Photo] PRIMARY KEY CLUSTERED ([Id_Menu] ASC),
    CONSTRAINT [FK_Menu_Photo_Restaurants_Menu] FOREIGN KEY ([Id_Menu]) REFERENCES [dbo].[Restaurants_Menu] ([Id])
);

