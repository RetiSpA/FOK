CREATE TABLE [dbo].[Order] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Create_Date]        DATETIME2 (7)   NOT NULL,
    [Id_Restaurant]      INT             NOT NULL,
    [Restaurant_Name]    NVARCHAR (50)   NULL,
    [Id_User]            INT             NOT NULL,
    [User_Name]          NVARCHAR (50)   NULL,
    [Id_Status]          INT             NOT NULL,
    [Price]              DECIMAL (18, 2) NOT NULL,
    [Restaurant_Address] NVARCHAR (100)  NULL,
    [Restaurant_Position] [sys].[geography] NULL, 
    [Delivery_Address]   NVARCHAR (100)  NOT NULL,
    [Delivery_Position] [sys].[geography] NOT NULL, 
    [Delivery_Requested_Date] DATETIME2 NOT NULL, 
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Status] FOREIGN KEY ([Id_Status]) REFERENCES [dbo].[Status] ([Id])
);

