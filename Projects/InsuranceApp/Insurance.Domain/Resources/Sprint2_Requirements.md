# Insurance Backend – Sprint 2

## Overview

Sprint 2 extends the insurance backend application by introducing:

- Policies lifecycle management  
- Broker management  
- Currencies and metadata configuration  
- Premium calculation logic  
- Aggregated reporting for administrators  

The implementation follows a strict **API First** approach and maintains clear separation of concerns between Domain, Application, Infrastructure, and API layers.

---

# 🎯 Goals

1. Extend domain models with:
   - Policies
   - Brokers
   - Currencies
   - Fee configurations
   - Risk factor configurations

2. Expose REST API endpoints for:
   - Broker flows (policy lifecycle)
   - Admin flows (metadata management, reports)

3. Follow API First development strategy.

---

# API First Development Order

## Phase 1 – Scaffolding

1. API contract (endpoints + request/response models)
2. DTOs (Application layer)
3. Controllers (API layer)  
   - No business logic  
   - Only service interfaces used
4. Service interfaces (Application layer)  
   - Only method signatures  
   - No implementation  

At this stage, the application should compile and run.

---

## Phase 2 – Implementation (Reverse Order)

1. Implement Domain models  
2. Implement Infrastructure (repositories, DB context)  
3. Implement Application services (business logic)  
4. Finalize Controllers, middleware, authentication, error handling  

---

# Business Rules

## Policies

- A policy starts in **Draft** status.  
- Only Draft policies can be activated.  
- A policy cannot be activated if:
  - It already ended.
  - There is already an active policy for the same building.
- Only Active policies can be cancelled.  
- A cancelled policy cannot be cancelled again.  
- Only Draft policies can be updated.  
- Policy number is generated automatically.  
- Final premium is calculated using:
  - Base premium
  - Broker commission
  - Active fee configurations
  - Active risk factors
  - Currency exchange rate

---

## Brokers

- Brokers can be **Active** or **Inactive**.  
- Only Active brokers can create policies.  
- An already active broker cannot be activated again.  
- An already inactive broker cannot be deactivated again.  

---

## Currencies

- Currency must be active to be used.  
- A currency used by active policies cannot be deactivated.  
- Status transitions must be validated (already active/inactive protection).  

---

## Fees & Risk Factors

- Only active configurations are used in premium calculation.  
- Status changes must prevent duplicate transitions.  

---

## Reports

Reports must provide aggregated data:

- Total policy count  
- Total final premium  
- Total final premium in base currency  

Grouping supported by:

- Country  
- County  
- City  
- Broker  

Filters supported:

- Date range  
- Policy status  
- Currency  
- Optional building type  

---

# Validation

FluentValidation is used for:

- Required fields  
- Positive numeric values  
- Date range validation (StartDate < EndDate)  
- StartDate not in the past  
- Enum validation  
- Simple field combinations  

---

# Architecture

The project follows a clean layered architecture:

- **Domain** – Core entities and business rules  
- **Application** – DTOs, services, interfaces, validation  
- **Infrastructure** – EF Core, repositories, database logic  
- **API** – Controllers, middleware, authentication  

Controllers contain no business logic.  
Repositories follow the Repository pattern and are accessed via UnitOfWork.

---

# Testing

- Unit tests for:
  - Services
  - Validators
  - Mapping profiles
  - Premium calculation logic  
- Integration tests for:
  - Full policy lifecycle  
  - Reports  
  - Broker activation/deactivation  
- Minimum **80% code coverage**

---

# Functional Requirements

## Broker Flows

- Create Draft Policy  
- Activate Policy  
- Cancel Policy  
- Update Policy (Draft only)  
- Get Policy  
- Search Policies (filters + pagination)  

## Admin Flows

- Create / Update Currency  
- Create / Update Fee Configuration  
- Create / Update Risk Factor Configuration  
- Activate / Deactivate Broker  
- Generate Reports  

---

# API Endpoints (High-Level)

## Broker

POST   /api/brokers/policies  
PUT    /api/brokers/policies/{policyId}  
POST   /api/brokers/policies/{policyId}/activate  
POST   /api/brokers/policies/{policyId}/cancel  
GET    /api/brokers/policies/{policyId}  
GET    /api/brokers/policies  

## Admin

GET    /api/admin/reports/policies-by-country  
GET    /api/admin/reports/policies-by-county  
GET    /api/admin/reports/policies-by-city  
GET    /api/admin/reports/policies-by-broker  

POST   /api/admin/currencies  
PUT    /api/admin/currencies/{currencyId}  

POST   /api/admin/fees  
PUT    /api/admin/fees/{feeId}  

POST   /api/admin/risk-factors  
PUT    /api/admin/risk-factors/{riskId}  

POST   /api/admin/brokers/{brokerId}/activate  
POST   /api/admin/brokers/{brokerId}/deactivate  

---

# Status Codes

- 200 OK – Success  
- 201 Created – Resource created  
- 204 No Content – Successful update  
- 400 Bad Request – Validation error  
- 401 Unauthorized  
- 403 Forbidden  
- 404 Not Found  
- 500 Internal Server Error  