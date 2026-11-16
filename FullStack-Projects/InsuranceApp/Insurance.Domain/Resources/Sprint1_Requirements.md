# Insurance Backend – Sprint 1

## Overview

Sprint 1 focuses on building the foundational structure of the insurance backend application, covering:

- Client management
- Building management
- Geography structure (Country → County → City)
- API First development approach
- Clean architecture layering

---

# Goals

1. Clear domain models (Domain layer)

2. REST API endpoints (Controllers)

3. API First approach

---

## API First – Development Order

### Phase 1 – Scaffolding

1. API contract (endpoints + request/response models)
2. DTOs (Application layer)
3. Controllers (API layer) – without business logic  
   - Only `Infra.Interfaces.Services` are used  
   - No service implementation at this stage  
   - Only interfaces and method signatures
4. Services (Application layer) – method signatures only  
   - No full implementation  
   - The application must compile and run

5. Domain models
6. Infrastructure (DB context, repositories)

After scaffolding everything (steps 1–4), implementation continues in reverse order:

- Implement Domain
- Implement Infrastructure
- Implement Application services
- Finalize Controllers, exceptions, API user handling, and any additional configuration

---

# Business Rules

1. Unique CNP (identifier must be unique).
2. The identifier should not be easily changed.
3. A client can own multiple buildings (Foreign Key relationship).
4. A building must be linked to an existing city.
5. The client cannot be changed; a building belongs to a single client.
6. A city belongs to a single county.
7. A county belongs to a single country.
8. FluentValidation can be used for:
   - Required fields
   - Email format validation
   - CNP length validation
   - Positive numeric values
   - Simple field combinations

---

# Extensibility

The code must be easy to extend across:

- API layer
- Services layer
- Domain layer
- Infrastructure layer

---

# Testing

- Unit tests for business logic
- Integration tests for API endpoints
- Minimum **80% code coverage**

---

# Code Quality

- Clear naming conventions
- Readable and maintainable structure
- Proper separation of concerns
- No duplicated logic

---

# Functional Requirements

1. CreateClient
2. UpdateClient
3. GetClients
4. GetClient
5. SearchClient
6. CreateBuilding
7. UpdateBuilding
8. GetBuildings
9. GetBuilding

---

# API Specification

## Clients

GET  /api/brokers/clients?name={name}&identifier={idNumber}  
GET  /api/brokers/clients/{clientId}  
POST /api/brokers/clients  
PUT  /api/brokers/clients/{clientId}  

## Buildings

GET  /api/brokers/clients/{clientId}/buildings  
GET  /api/brokers/buildings/{buildingId}  
POST /api/brokers/clients/{clientId}/buildings  
PUT  /api/brokers/buildings/{buildingId}  

## Geography

GET  /api/brokers/countries  
GET  /api/brokers/countries/{countryId}/counties  
GET  /api/brokers/counties/{countyId}/cities  

---

# Status Codes

- 200 OK – Success
- 201 Created – Resource created
- 400 Bad Request – Validation errors
- 404 Not Found
- 500 Internal Server Error
