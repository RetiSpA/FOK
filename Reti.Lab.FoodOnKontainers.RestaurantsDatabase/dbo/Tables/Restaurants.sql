CREATE TABLE [dbo].[Restaurants] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (50)   NOT NULL,
    [Address]            NVARCHAR (100)  NOT NULL,
    [Position] [sys].[geography]		 NOT NULL, 
    [Email]              NVARCHAR (50)   NOT NULL,
    [PhoneNumber]        NVARCHAR(50)    NOT NULL,
    [Id_Restaurant_Type] INT             NOT NULL,
    [Enabled]            BIT             CONSTRAINT [DF_Restaurants_Enabled] DEFAULT ((1)) NOT NULL,
    [Average_Rating]     DECIMAL (5, 2)  NULL,
    CONSTRAINT [PK_Restaurants] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Restaurants_Restaurant_Types] FOREIGN KEY ([Id_Restaurant_Type]) REFERENCES [dbo].[Restaurant_Types] ([Id])
);

