USE InsuranceDb;
GO

CREATE UNIQUE INDEX UX_Counties_Country_Name
ON Counties (CountryId, Name);

CREATE UNIQUE INDEX UX_Cities_County_Code
ON Cities (CountyId, Code);

CREATE UNIQUE INDEX UX_Clients_Cnp
ON Clients (Cnp)
WHERE Cnp IS NOT NULL;

CREATE UNIQUE INDEX UX_Clients_Cui
ON Clients (Cui)
WHERE Cui IS NOT NULL;

CREATE INDEX IX_Buildings_ClientId
ON Buildings (ClientId);

CREATE INDEX IX_Buildings_CityId
ON Buildings (CityId);

CREATE INDEX IX_Buildings_BuildingTypeId
ON Buildings (BuildingTypeId);
GO
