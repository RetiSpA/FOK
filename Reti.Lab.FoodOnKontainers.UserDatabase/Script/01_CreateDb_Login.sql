USE [master]
GO
CREATE LOGIN [userSvc] WITH PASSWORD=N'PasswordUser01!', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
use [Fok_User]

-- LOGIN
GO
USE [Fok_User]
GO
CREATE USER [userSvc] FOR LOGIN [userSvc]
GO
USE [Fok_User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [userSvc]
GO
USE [Fok_User]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [userSvc]
GO
