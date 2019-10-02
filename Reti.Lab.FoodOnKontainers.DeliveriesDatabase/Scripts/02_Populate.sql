USE [Fok_Deliveries]
GO

INSERT INTO [dbo].[Status]
           ([Id]
           ,[Name])
     VALUES
           (1,'To Pick Up')
		  ,(2,'Picked Up')
		  ,(3,'Delivered')
		  ,(4,'Canceled')
GO
