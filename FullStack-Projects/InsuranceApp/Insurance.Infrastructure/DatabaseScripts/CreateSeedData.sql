USE InsuranceDb;
GO

DELETE FROM Buildings;
DELETE FROM BuildingTypes;
DELETE FROM CompanyClients;
DELETE FROM IndividualClients;
DELETE FROM Clients;
DELETE FROM Cities;
DELETE FROM Counties;
DELETE FROM Countries;
GO

DECLARE @CountryId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Countries (Id, Name)
VALUES (@CountryId, 'Romania');

DECLARE @CountyId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Counties (Id, CountryId, Name)
VALUES (@CountyId, @CountryId, 'Bucharest');

DECLARE @CityId UNIQUEIDENTIFIER = NEWID();
INSERT INTO Cities (Id, CountyId, Name, Code)
VALUES (@CityId, @CountyId, 'Bucharest', 'RO-B');

INSERT INTO BuildingTypes (Id, Name)
VALUES 
 (NEWID(), 'Residential'),
 (NEWID(), 'Office'),
 (NEWID(), 'Industrial');
