USE InsuranceDb;
GO

DELETE FROM Policies;
DELETE FROM Buildings;
DELETE FROM Brokers;
DELETE FROM Administrators;
DELETE FROM Clients;
DELETE FROM BuildingTypes;
DELETE FROM Cities;
DELETE FROM Counties;
DELETE FROM Countries;
DELETE FROM Currencies;
DELETE FROM ClientTypes;

SET IDENTITY_INSERT ClientTypes ON;

INSERT INTO ClientTypes (Id, Code, Name)
VALUES 
(0,'INDIVIDUAL','Individual'),
(1,'COMPANY','Company');

SET IDENTITY_INSERT ClientTypes OFF;


DECLARE @CountryRO UNIQUEIDENTIFIER = NEWID();
DECLARE @CountryDE UNIQUEIDENTIFIER = NEWID();

INSERT INTO Countries VALUES 
(@CountryRO,'Romania'),
(@CountryDE,'Germany');


DECLARE @CountyBuc UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyCluj UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyIasi UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyBerlin UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyMunich UNIQUEIDENTIFIER = NEWID();

INSERT INTO Counties VALUES
(@CountyBuc,@CountryRO,'Bucharest'),
(@CountyCluj,@CountryRO,'Cluj'),
(@CountyIasi,@CountryRO,'Iasi'),
(@CountyBerlin,@CountryDE,'Berlin'),
(@CountyMunich,@CountryDE,'Bavaria');


DECLARE @CityBuc UNIQUEIDENTIFIER = NEWID();
DECLARE @CityCluj UNIQUEIDENTIFIER = NEWID();
DECLARE @CityIasi UNIQUEIDENTIFIER = NEWID();
DECLARE @CityBerlin UNIQUEIDENTIFIER = NEWID();
DECLARE @CityMunich UNIQUEIDENTIFIER = NEWID();

INSERT INTO Cities VALUES
(@CityBuc,@CountyBuc,'Bucharest','RO-B'),
(@CityCluj,@CountyCluj,'Cluj-Napoca','RO-CJ'),
(@CityIasi,@CountyIasi,'Iasi','RO-IS'),
(@CityBerlin,@CountyBerlin,'Berlin','DE-BE'),
(@CityMunich,@CountyMunich,'Munich','DE-M');


DECLARE @Residential UNIQUEIDENTIFIER = NEWID();
DECLARE @Office UNIQUEIDENTIFIER = NEWID();
DECLARE @Industrial UNIQUEIDENTIFIER = NEWID();

INSERT INTO BuildingTypes VALUES
(@Residential,'Residential'),
(@Office,'Office'),
(@Industrial,'Industrial');


DECLARE @RON UNIQUEIDENTIFIER = NEWID();
DECLARE @EUR UNIQUEIDENTIFIER = NEWID();
DECLARE @USD UNIQUEIDENTIFIER = NEWID();

INSERT INTO Currencies VALUES
(@RON,'RON','Romanian Leu',1.0000,1),
(@EUR,'EUR','Euro',4.9500,1),
(@USD,'USD','US Dollar',4.5000,1);


INSERT INTO Administrators VALUES
(NEWID(),'System Administrator','admin@insurance.local','Admin');


DECLARE @BrokerIds TABLE (Id UNIQUEIDENTIFIER);

INSERT INTO Brokers
OUTPUT inserted.Id INTO @BrokerIds
VALUES
(NEWID(),'BR-001','Andrei Dumitrescu','andrei@broker.ro','0722000001',2.5,'Active'),
(NEWID(),'BR-002','Raluca Marinescu','raluca@broker.ro','0722000002',2.7,'Active'),
(NEWID(),'BR-003','Mihai Popa','mihai@broker.ro','0722000003',2.6,'Inactive'),
(NEWID(),'BR-004','Cristina Ionescu','cristina@broker.ro','0722000004',2.9,'Active'),
(NEWID(),'BR-005','Vlad Stan','vlad@broker.ro','0722000005',3.0,'Active');


DECLARE @ClientIds TABLE (Id UNIQUEIDENTIFIER);

INSERT INTO Clients (Id,ClientTypeId,Name,Cnp)
OUTPUT inserted.Id INTO @ClientIds
VALUES
(NEWID(),0,'Ion Popescu','1800101223344'),
(NEWID(),0,'Maria Ionescu','2850202334455'),
(NEWID(),0,'Daniel Matei','1900517667788');

INSERT INTO Clients (Id,ClientTypeId,Name,Cui)
OUTPUT inserted.Id INTO @ClientIds
VALUES
(NEWID(),1,'TechVision SRL','RO1234561'),
(NEWID(),1,'ConstructMax SRL','RO1234562');


DECLARE @Buildings TABLE (Id UNIQUEIDENTIFIER);

INSERT INTO Buildings
OUTPUT inserted.Id INTO @Buildings
VALUES
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBuc,@Residential,'Calea Victoriei','12',1985,2,85,160000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityCluj,@Office,'Memorandumului','45',2008,5,320,600000,0,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityIasi,@Industrial,'Chimiei','8',2012,1,1200,2100000,1,0,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBerlin,@Office,'Alexanderplatz','5',2015,10,950,1750000,0,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityMunich,@Residential,'Marienplatz','10',2003,3,150,260000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME());


DECLARE @i INT = 1;

WHILE @i <= 40
BEGIN
    INSERT INTO Policies
    SELECT
        NEWID(),
        CONCAT('POL-',FORMAT(@i,'000')),
        (SELECT TOP 1 Id FROM @Buildings ORDER BY NEWID()),
        (SELECT TOP 1 Id FROM @BrokerIds ORDER BY NEWID()),
        (SELECT TOP 1 Id FROM (VALUES(@RON),(@EUR),(@USD)) AS C(Id) ORDER BY NEWID()),
        CASE 
            WHEN @i % 6 = 0 THEN 'Cancelled'
            WHEN @i % 4 = 0 THEN 'Expired'
            ELSE 'Active'
        END,
        DATEFROMPARTS(2023 + (@i % 3), (@i % 12) + 1, 1),
        DATEFROMPARTS(2024 + (@i % 2), (@i % 12) + 1, 1),
        1000 + (@i * 120),
        1300 + (@i * 170),
        SYSUTCDATETIME(),
        NULL,
        NULL;

    SET @i = @i + 1;
END
