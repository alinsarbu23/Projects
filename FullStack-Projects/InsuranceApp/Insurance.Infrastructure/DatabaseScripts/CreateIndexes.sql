USE InsuranceDb;
GO


CREATE UNIQUE INDEX UX_ClientTypes_Code
ON ClientTypes (Code);
GO

CREATE UNIQUE INDEX UX_Countries_Name
ON Countries (Name);
GO

CREATE UNIQUE INDEX UX_Counties_Country_Name
ON Counties (CountryId, Name);
GO

CREATE UNIQUE INDEX UX_Cities_County_Code
ON Cities (CountyId, Code);
GO


CREATE UNIQUE INDEX UX_Clients_Cnp
ON Clients (Cnp)
WHERE Cnp IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_Clients_Cui
ON Clients (Cui)
WHERE Cui IS NOT NULL;
GO


CREATE INDEX IX_Buildings_ClientId
ON Buildings (ClientId);
GO

CREATE INDEX IX_Buildings_CityId
ON Buildings (CityId);
GO

CREATE INDEX IX_Buildings_BuildingTypeId
ON Buildings (BuildingTypeId);
GO
