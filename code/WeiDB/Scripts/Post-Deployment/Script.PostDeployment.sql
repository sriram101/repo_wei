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

DBCC CHECKIDENT (Status, RESEED, 0)

go

INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('UnProcessed','', true)
INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('Translated','Message with CTC Codes translated successfully', false)
INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('Review','Message needs review for translation', true)
INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('SentForOFACCheck','Message sent for Watchlist Filtering Check', false)
INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('OFACResponseReceived','Response Received from Watchlist Filtering Program', false)
INSERT INTO Status (Description,DisplayName,ShowStatus)
VALUES('Processed','Message Processing Complete', true)