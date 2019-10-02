USE [Fok_Orders]
GO

INSERT INTO [dbo].[Status]
           ([Id]
           ,[Name])
     VALUES
           (1,'Inserted')
		  ,(2,'Accepted')
		  ,(3,'Rejected')
		  ,(4,'Delivering')
		  ,(5,'Completed')
		  ,(6,'Canceled')
GO
