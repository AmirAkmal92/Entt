 -- =============================================
-- Create database template
-- =============================================
USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'EnttPlatform'
)
DROP DATABASE EnttPlatform
GO

CREATE DATABASE [EnttPlatform]
ON
PRIMARY  
    (NAME = Arch1,
    FILENAME = 'c:\data\EnttPlatform.mdf',
    SIZE = 100MB,
    MAXSIZE = 200,
    FILEGROWTH = 20)
LOG ON 
   (NAME = Archlog1,
    FILENAME = 'c:\data\EnttPlatform_log.ldf',
    SIZE = 100MB,
    MAXSIZE = 200,
    FILEGROWTH = 20);
GO
