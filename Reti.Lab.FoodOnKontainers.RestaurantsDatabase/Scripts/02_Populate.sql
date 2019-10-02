-- Dish Types

USE [Fok_Restaurants]
GO

INSERT INTO [dbo].[Dish_Types]
           ([Id]
           ,[Name])
     VALUES
           (1 ,'Panino'),
		   (2 ,'Primo piatto'),
		   (3 ,'Secondo piatto'),
		   (4 ,'Antipasto'),
		   (5 ,'Dolce'),
		   (6 ,'Contorno'),
		   (7 ,'Piadina'),
		   (8 ,'Pizza')

GO

-- Restaurants Types

USE [Fok_Restaurants]
GO

INSERT INTO [dbo].[Restaurant_Types]
           ([Id]
           ,[Name])
     VALUES
           (1 ,'Paninoteca'),
		   (2 ,'Ristorante'),
		   (3 ,'Trattoria'),
		   (4 ,'Tavola calda'),
		   (5 ,'Piadineria'),
		   (6 ,'Ristorante argentino'),
		   (7 ,'Rosticceria'),
		   (8 ,'Pizzeria'),
		   (9 ,'Ristorante cinese'),
		   (10 ,'Ristorante giapponese')
		   
GO


