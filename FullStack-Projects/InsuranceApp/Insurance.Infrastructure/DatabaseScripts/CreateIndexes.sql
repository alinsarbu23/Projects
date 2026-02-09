USE InsuranceDb;
GO

CREATE UNIQUE INDEX UX_ClientTypes_Code ON ClientTypes(Code);
GO
CREATE UNIQUE INDEX UX_Countries_Name ON Countries(Name);
GO
CREATE UNIQUE INDEX UX_Counties_Country_Name ON Counties(CountryId, Name);
GO
CREATE UNIQUE INDEX UX_Cities_County_Code ON Cities(CountyId, Code);
GO

CREATE UNIQUE INDEX UX_Clients_Cnp ON Clients(Cnp) WHERE Cnp IS NOT NULL;
GO
CREATE UNIQUE INDEX UX_Clients_Cui ON Clients(Cui) WHERE Cui IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_Brokers_Code ON Brokers(BrokerCode);
GO
CREATE UNIQUE INDEX UX_Currencies_Code ON Currencies(Code);
GO

CREATE INDEX IX_Buildings_ClientId ON Buildings(ClientId);
GO
CREATE INDEX IX_Policies_BrokerId ON Policies(BrokerId);
GO
CREATE INDEX IX_Policies_BuildingId ON Policies(BuildingId);
GO
