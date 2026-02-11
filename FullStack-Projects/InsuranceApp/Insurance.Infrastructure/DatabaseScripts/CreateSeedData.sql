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


DECLARE @CountryId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Countries VALUES (@CountryId,'Romania');

DECLARE @CountyBuc UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyCluj UNIQUEIDENTIFIER = NEWID();
DECLARE @CountyIasi UNIQUEIDENTIFIER = NEWID();

INSERT INTO Counties VALUES
(@CountyBuc,@CountryId,'Bucharest'),
(@CountyCluj,@CountryId,'Cluj'),
(@CountyIasi,@CountryId,'Iasi');

DECLARE @CityBuc UNIQUEIDENTIFIER = NEWID();
DECLARE @CityCluj UNIQUEIDENTIFIER = NEWID();
DECLARE @CityIasi UNIQUEIDENTIFIER = NEWID();

INSERT INTO Cities VALUES
(@CityBuc,@CountyBuc,'Bucharest','RO-B'),
(@CityCluj,@CountyCluj,'Cluj-Napoca','RO-CJ'),
(@CityIasi,@CountyIasi,'Iasi','RO-IS');


DECLARE @Residential UNIQUEIDENTIFIER = NEWID();
DECLARE @Office UNIQUEIDENTIFIER = NEWID();
DECLARE @Industrial UNIQUEIDENTIFIER = NEWID();

INSERT INTO BuildingTypes VALUES
(@Residential,'Residential'),
(@Office,'Office'),
(@Industrial,'Industrial');


DECLARE @RON UNIQUEIDENTIFIER = NEWID();
INSERT INTO Currencies VALUES
(@RON,'RON','Romanian Leu',1.0000,1);


INSERT INTO Administrators VALUES
(NEWID(),'System Administrator','admin@insurance.local','Admin');


DECLARE @BrokerIds TABLE (Id UNIQUEIDENTIFIER);

INSERT INTO Brokers
OUTPUT inserted.Id INTO @BrokerIds
VALUES
(NEWID(),'BR-001','Andrei Dumitrescu','andrei.dumitrescu@broker.ro','0722000001',2.5,'Active'),
(NEWID(),'BR-002','Raluca Marinescu','raluca.marinescu@broker.ro','0722000002',2.7,'Active'),
(NEWID(),'BR-003','Mihai Popa','mihai.popa@broker.ro','0722000003',2.6,'Active'),
(NEWID(),'BR-004','Cristina Ionescu','cristina.ionescu@broker.ro','0722000004',2.9,'Active'),
(NEWID(),'BR-005','Vlad Stan','vlad.stan@broker.ro','0722000005',3.0,'Active'),
(NEWID(),'BR-006','Oana Gheorghe','oana.gheorghe@broker.ro','0722000006',2.8,'Active'),
(NEWID(),'BR-007','Alexandru Pavel','alex.pavel@broker.ro','0722000007',2.4,'Active'),
(NEWID(),'BR-008','Daniela Rusu','daniela.rusu@broker.ro','0722000008',2.6,'Active'),
(NEWID(),'BR-009','Stefan Munteanu','stefan.munteanu@broker.ro','0722000009',2.5,'Active'),
(NEWID(),'BR-010','Laura Petrescu','laura.petrescu@broker.ro','0722000010',2.7,'Active');


DECLARE @ClientIds TABLE (Id UNIQUEIDENTIFIER);


INSERT INTO Clients (Id,ClientTypeId,Name,Cnp)
OUTPUT inserted.Id INTO @ClientIds
VALUES
(NEWID(),0,'Ion Popescu','1800101223344'),
(NEWID(),0,'Maria Ionescu','2850202334455'),
(NEWID(),0,'Adrian Georgescu','1750315445566'),
(NEWID(),0,'Elena Stan','2860406556677'),
(NEWID(),0,'Daniel Matei','1900517667788');


INSERT INTO Clients (Id,ClientTypeId,Name,Cui)
OUTPUT inserted.Id INTO @ClientIds
VALUES
(NEWID(),1,'TechVision SRL','RO1234561'),
(NEWID(),1,'ConstructMax SRL','RO1234562'),
(NEWID(),1,'GreenEnergy Solutions SRL','RO1234563'),
(NEWID(),1,'Urban Offices SA','RO1234564'),
(NEWID(),1,'Industrial Logistic SRL','RO1234565');


DECLARE @Buildings TABLE (Id UNIQUEIDENTIFIER);

INSERT INTO Buildings
OUTPUT inserted.Id INTO @Buildings
VALUES
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBuc,@Residential,'Calea Victoriei','12',1985,2,85,160000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityCluj,@Office,'Str. Memorandumului','45',2008,5,320,600000,0,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityIasi,@Industrial,'Bd. Chimiei','8',2012,1,1200,2100000,1,0,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBuc,@Office,'Piata Unirii','3',2015,10,950,1750000,0,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityCluj,@Residential,'Str. Buna Ziua','18',2001,2,95,190000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityIasi,@Residential,'Str. Stefan cel Mare','9',1978,1,70,110000,1,0,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBuc,@Residential,'Drumul Taberei','44',1999,4,130,210000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityCluj,@Office,'Str. Dorobantilor','22',2010,6,500,850000,0,1,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityIasi,@Industrial,'Zona Industriala Vest','15',2018,1,1500,2500000,1,0,SYSUTCDATETIME(),SYSUTCDATETIME()),
(NEWID(),(SELECT TOP 1 Id FROM @ClientIds ORDER BY NEWID()),@CityBuc,@Residential,'Bd. Unirii','101',2003,3,150,260000,1,1,SYSUTCDATETIME(),SYSUTCDATETIME());


DECLARE @i INT = 1;

WHILE @i <= 10
BEGIN
    INSERT INTO Policies
    SELECT
        NEWID(),
        CONCAT('POL-2025-',FORMAT(@i,'000')),
        (SELECT TOP 1 Id FROM @Buildings ORDER BY NEWID()),
        (SELECT TOP 1 Id FROM @BrokerIds ORDER BY NEWID()),
        @RON,
        CASE 
            WHEN @i IN (3,7) THEN 'Expired'
            WHEN @i IN (5,9) THEN 'Cancelled'
            ELSE 'Active'
        END,
        DATEFROMPARTS(2025,1,@i),
        DATEFROMPARTS(2025,12,@i),
        1200 + (@i * 150),
        1500 + (@i * 200),
        SYSUTCDATETIME(),
        NULL,
        NULL;

    SET @i = @i + 1;
END
