USE [master]
GO

/****** REPLACE BEGIN******/
IF EXISTS(SELECT name FROM sys.databases
    WHERE name = 'WPFCSHARP')
    DROP DATABASE WPFCSHARP
GO

CREATE DATABASE [WPFCSHARP]
GO

ALTER DATABASE [WPFCSHARP] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WPFCSHARP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

USE [WPFCSHARP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT name FROM WPFCSHARP.sys.tables
    WHERE name = 'TestTable')
	DROP TABLE [dbo].[TestTable];
GO


GO
CREATE TABLE [dbo].[TestTable] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
);
