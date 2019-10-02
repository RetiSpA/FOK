CREATE TABLE [dbo].[Rider] (
    [Id_Rider]       INT               IDENTITY (1, 1) NOT NULL,
    [Rider_Name]     NVARCHAR (50)     NOT NULL,
	[Starting_Point_Address] NVARCHAR (100) NULL,
    [Starting_Point] [sys].[geography] NULL,
    [Range]          INT               NULL,
    [Average_Rating] DECIMAL (5, 2)    NULL,
    [Active]         BIT               NOT NULL,
    CONSTRAINT [PK_Rider] PRIMARY KEY CLUSTERED ([Id_Rider] ASC)
);

