USE InsuranceDb;
GO

CREATE UNIQUE INDEX UX_Counties_Country_Name
ON Counties (CountryId, Name);

CREATE UNIQUE INDEX UX_Cities_County_Code
ON Cities (CountyId, Code);

CREATE UNIQUE INDEX UX_IndividualClients_Cnp
ON IndividualClients (Cnp);

CREATE UNIQUE INDEX UX_CompanyClients_Cui
ON CompanyClients (Cui);

CREATE UNIQUE INDEX UX_BuildingTypes_Name
ON BuildingTypes (Name);

CREATE INDEX IX_Buildings_ClientId
ON Buildings (ClientId);

CREATE INDEX IX_Buildings_CityId
ON Buildings (CityId);

CREATE INDEX IX_Buildings_BuildingTypeId
ON Buildings (BuildingTypeId);
