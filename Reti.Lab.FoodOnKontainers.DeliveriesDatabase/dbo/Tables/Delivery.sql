CREATE TABLE [dbo].[Delivery] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Id_Order]         INT            NOT NULL,
    [Id_Rider]         INT            NULL,
    [Take_Charge_Date] DATETIME2 (7)  NULL,
    [PickUp_Date]      DATETIME2 (7)  NULL,
    [Delivery_Date]    DATETIME2 (7)  NULL,
    [Delivery_Requested_Date] DATETIME2 NOT NULL, 
    [PickUp_Address]   NVARCHAR (100) NOT NULL,
	[PickUp_Position]   [sys].[geography] NOT NULL,
    [Delivery_Address] NVARCHAR (100) NOT NULL,
	[Delivery_Position]   [sys].[geography] NOT NULL,
    [Id_Status]        INT            NOT NULL,
    [Id_Restaurant] INT NULL, 
    [Restaurant_Name] NVARCHAR(50) NOT NULL, 
    [Delivery_Name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Delivery] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Delivery_Rider] FOREIGN KEY ([Id_Rider]) REFERENCES [dbo].[Rider] ([Id_Rider]),
    CONSTRAINT [FK_Delivery_Status] FOREIGN KEY ([Id_Status]) REFERENCES [dbo].[Status] ([Id])
);

