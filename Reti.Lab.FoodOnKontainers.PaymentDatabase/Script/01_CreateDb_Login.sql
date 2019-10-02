USE [master]
GO
CREATE LOGIN [paymentSvc] WITH PASSWORD=N'PasswordPayment01!', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
use [Fok_Payment]

-- LOGIN
GO
USE [Fok_Payment]
GO
CREATE USER [paymentSvc] FOR LOGIN [paymentSvc]
GO
USE [Fok_Payment]
GO
ALTER ROLE [db_datareader] ADD MEMBER [paymentSvc]
GO
USE [Fok_Payment]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [paymentSvc]
GO
