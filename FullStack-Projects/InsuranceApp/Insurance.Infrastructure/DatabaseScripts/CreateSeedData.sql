USE InsuranceDb;
GO

INSERT INTO ClientTypes (Code, Name)
VALUES ('INDIVIDUAL','Individual'), ('COMPANY','Company');
GO

DECLARE @CountryId UNIQUEIDENTIFIER = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
DECLARE @CountyId UNIQUEIDENTIFIER = 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb';
DECLARE @CityId UNIQUEIDENTIFIER = 'cccccccc-cccc-cccc-cccc-cccccccccccc';

INSERT INTO Countries VALUES (@CountryId,'Romania');
INSERT INTO Counties VALUES (@CountyId,@CountryId,'Bucharest');
INSERT INTO Cities VALUES (@CityId,@CountyId,'Bucharest','RO-B');
GO

INSERT INTO BuildingTypes VALUES
 ('11111111-1111-1111-1111-111111111111','Residential'),
 ('22222222-2222-2222-2222-222222222222','Office'),
 ('33333333-3333-3333-3333-333333333333','Industrial');
GO

INSERT INTO Administrators VALUES
 (NEWID(),'System Admin','admin@insurance.local','Admin');
GO

DECLARE @BrokerId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Brokers VALUES
 (@BrokerId,'BR-001','Alpha Broker','alpha@broker.ro','0700000001',2.5,'Active');
GO

DECLARE @CurrencyId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Currencies VALUES
 (@CurrencyId,'RON','Romanian Leu',1.0000,1);
GO

DECLARE @ClientId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Clients (Id,ClientTypeId,Name)
VALUES (@ClientId,1,'Ion Popescu');
GO

DECLARE @BuildingId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Buildings
 (Id,ClientId,CityId,BuildingTypeId,Street,StreetNumber,SurfaceArea,InsuredValue)
VALUES
 (@BuildingId,@ClientId,@CityId,'11111111-1111-1111-1111-111111111111',
  'Main Street','10',75,150000);
GO

INSERT INTO Policies
VALUES
 (NEWID(),'POL-0001',@BuildingId,@BrokerId,@CurrencyId,
  'Active','2025-01-01','2025-12-31',1000,1150,SYSUTCDATETIME(),NULL,NULL);
GO
