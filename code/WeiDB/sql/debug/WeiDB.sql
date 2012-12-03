/*
Deployment script for Wei
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Wei"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\"

GO
USE [master]

GO
:on error exit
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO

IF NOT EXISTS (SELECT 1 FROM [master].[dbo].[sysdatabases] WHERE [name] = N'$(DatabaseName)')
BEGIN
    RAISERROR(N'You cannot deploy this update script to target W2K3-SQL2K5. The database for which this script was built, Wei, does not exist on this server.', 16, 127) WITH NOWAIT
    RETURN
END

GO

IF (@@servername != 'W2K3-SQL2K5')
BEGIN
    RAISERROR(N'The server name in the build script %s does not match the name of the target server %s. Verify whether your database project settings are correct and whether your build script is up to date.', 16, 127,N'W2K3-SQL2K5',@@servername) WITH NOWAIT
    RETURN
END

GO

IF CAST(DATABASEPROPERTY(N'$(DatabaseName)','IsReadOnly') as bit) = 1
BEGIN
    RAISERROR(N'You cannot deploy this update script because the database for which it was built, %s , is set to READ_ONLY.', 16, 127, N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
USE [$(DatabaseName)]

GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


delete from Handlers
go

DBCC CHECKIDENT (Handlers, RESEED, 0)

go

INSERT INTO Handlers ( name, dll, type)
	VALUES ('PRIME', 'PrimeHandler.dll', 'Handlers.PrimeHandler')
go

delete from Drivers
go

DBCC CHECKIDENT (Drivers, RESEED, 0)

go
INSERT INTO Drivers (name, dll, type)
	VALUES ('File Driver', 'FileDriver.dll', 'Drivers.FileDriver')

go
INSERT INTO Drivers (name, dll, type)
	VALUES ('MQ Driver', 'MQDriver.dll', 'MQDriver.MQDriver')

delete from Interfaces
go

DBCC CHECKIDENT (Interfaces, RESEED, 0)

go

INSERT INTO Interfaces(name, handler, driver, fileformat, config)
	VALUES ( 'Driver1', 1, 1, 'SWIFT , XML, CHIPS', '<config><inputFilePattern>*.*</inputFilePattern><inputFolder>C:\wei\driver1\input</inputFolder><okFolder>C:\wei\driver1\ok</okFolder><confirmFolder>C:\wei\driver1\confirm</confirmFolder><errorFolder>C:\wei\driver1\error</errorFolder><ofacInputFolder>C:\wei\driver1\primeInput</ofacInputFolder><ofacOkFolder>C:\wei\driver1\primeOk</ofacOkFolder><ofacConfirmFolder>C:\wei\driver1\primeConfirm</ofacConfirmFolder></config>')

go

INSERT INTO Interfaces(name, handler, driver, fileformat, config)
	VALUES ( 'MQDriver1', 1, 2, 'SWIFT, XML, CHIPS', '<config><queueManager>QM_WEI</queueManager><inputQueue>InQueue</inputQueue><okQueue>OkQueue</okQueue><confirmQueue>ConfirmQueue</confirmQueue><errorQueue>ErrorQueue</errorQueue><ofacInputQueue>PrimeInQueue</ofacInputQueue><ofacOkQueue>PrimeOkQueue</ofacOkQueue><ofacConfirmQueue>PrimeConfirmQueue</ofacConfirmQueue></config>')

INSERT INTO Interfaces(name, handler, driver, fileformat, config)
	VALUES ( 'MQDriver2', 1, 2, 'SWIFT, XML, CHIPS', '<config><queueManager>QM_WEI</queueManager><inputQueue>InQueue2</inputQueue><okQueue>OkQueue2</okQueue><confirmQueue>ConfirmQueue2</confirmQueue><errorQueue>ErrorQueue2</errorQueue><ofacInputQueue>PrimeInQueue2</ofacInputQueue><ofacOkQueue>PrimeOkQueue2</ofacOkQueue><ofacConfirmQueue>PrimeConfirmQueue2</ofacConfirmQueue></config>')


GO
