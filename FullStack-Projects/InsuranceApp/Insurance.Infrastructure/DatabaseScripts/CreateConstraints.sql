

ALTER TABLE Counties
ADD CONSTRAINT FK_Counties_Countries
FOREIGN KEY (CountryId) REFERENCES Countries(Id);
GO

ALTER TABLE Cities
ADD CONSTRAINT FK_Cities_Counties
FOREIGN KEY (CountyId) REFERENCES Counties(Id);
GO

ALTER TABLE Clients
ADD CONSTRAINT FK_Clients_ClientTypes
FOREIGN KEY (ClientTypeId) REFERENCES ClientTypes(Id);
GO

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_Clients
FOREIGN KEY (ClientId) REFERENCES Clients(Id);
GO

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_Cities
FOREIGN KEY (CityId) REFERENCES Cities(Id);
GO

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_BuildingTypes
FOREIGN KEY (BuildingTypeId) REFERENCES BuildingTypes(Id);
GO

ALTER TABLE Policies
ADD CONSTRAINT FK_Policies_Buildings
FOREIGN KEY (BuildingId) REFERENCES Buildings(Id);
GO

ALTER TABLE Policies
ADD CONSTRAINT FK_Policies_Brokers
FOREIGN KEY (BrokerId) REFERENCES Brokers(Id);
GO

ALTER TABLE Policies
ADD CONSTRAINT FK_Policies_Currencies
FOREIGN KEY (CurrencyId) REFERENCES Currencies(Id);
GO

ALTER TABLE Buildings
ADD CONSTRAINT CK_Buildings_SurfaceArea CHECK (SurfaceArea > 0);
GO

ALTER TABLE Buildings
ADD CONSTRAINT CK_Buildings_InsuredValue CHECK (InsuredValue > 0);
GO
