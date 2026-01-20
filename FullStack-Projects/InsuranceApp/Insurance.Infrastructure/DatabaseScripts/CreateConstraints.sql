USE InsuranceDb;
GO

ALTER TABLE Counties
ADD CONSTRAINT FK_Counties_Countries
FOREIGN KEY (CountryId) REFERENCES Countries(Id);

ALTER TABLE Cities
ADD CONSTRAINT FK_Cities_Counties
FOREIGN KEY (CountyId) REFERENCES Counties(Id);

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_Clients
FOREIGN KEY (ClientId) REFERENCES Clients(Id);

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_Cities
FOREIGN KEY (CityId) REFERENCES Cities(Id);

ALTER TABLE Buildings
ADD CONSTRAINT FK_Buildings_BuildingTypes
FOREIGN KEY (BuildingTypeId) REFERENCES BuildingTypes(Id);

ALTER TABLE Clients
ADD CONSTRAINT CK_Clients_ClientType
CHECK (ClientType IN ('INDIVIDUAL', 'COMPANY'));

ALTER TABLE Buildings
ADD CONSTRAINT CK_Buildings_SurfaceArea
CHECK (SurfaceArea > 0);

ALTER TABLE Buildings
ADD CONSTRAINT CK_Buildings_InsuredValue
CHECK (InsuredValue > 0);
GO
