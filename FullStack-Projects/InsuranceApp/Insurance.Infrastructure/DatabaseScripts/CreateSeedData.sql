USE InsuranceDb;
GO



INSERT INTO ClientTypes (Code, Name)
VALUES
 ('INDIVIDUAL', 'Individual'),
 ('COMPANY', 'Company');
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
GO

INSERT INTO BuildingTypes (Id, Name)
VALUES
 (NEWID(), 'Residential'),
 (NEWID(), 'Office'),
 (NEWID(), 'Industrial');
GO
