# Sprint 3 – Advanced Features & Production Readiness

---

## 1. Overview

### Scenario

The insurance backend is now extended to include advanced features required for production readiness.

The system must now support:

- Authentication & Authorization
- Policy lifecycle automation (expiration handling)
- Advanced filtering & pagination
- Export functionality
- Soft delete & audit tracking
- Improved robustness and performance considerations

There are still two main user groups:

### 1. Brokers
- Manage clients and buildings
- Create and manage policies
- Search and filter policies
- View only their own data

### 2. Administrators
- Maintain metadata
- Manage brokers
- Access advanced reports
- Export report data

---

## 2. Learning Goals

By implementing Sprint 3, the learner should practice:

1. Implementing authentication (JWT-based).
2. Applying role-based authorization.
3. Enforcing data access restrictions per user role.
4. Implementing pagination and advanced filtering.
5. Designing extensible export mechanisms.
6. Handling soft delete and audit information.
7. Improving integration test robustness.
8. Writing cleaner, more production-ready code.

---

## 3. New Domain Enhancements

### Policy Enhancements

- Policies automatically transition to **Expired** when EndDate passes.
- Expired policies cannot be activated or cancelled.
- Policy history tracking (optional).

### Audit Fields (for all entities)

- CreatedAt
- UpdatedAt
- CreatedBy
- UpdatedBy
- IsDeleted (Soft delete)

### Soft Delete Rules

- Entities are not physically deleted.
- Queries must exclude soft-deleted records.
- Reports must ignore deleted records.

---

## 4. Functional Requirements

### 4.1 Authentication & Authorization

1. Implement JWT-based authentication.
2. Users must log in to receive a token.
3. Endpoints must be protected using roles:
   - Broker
   - Admin
4. Brokers:
   - Can only access their own policies.
5. Admin:
   - Can access all data.

---

### 4.2 Policy Expiration

1. Policies automatically become **Expired** if EndDate < today.
2. Expired policies:
   - Cannot be activated.
   - Cannot be cancelled.
3. Reports must correctly reflect expired policies.

---

### 4.3 Advanced Search & Pagination

Broker policy search must support:

- Filter by:
  - Status
  - Date range
  - Client
  - Building
  - Currency
- Pagination:
  - page
  - pageSize
- Sorting:
  - by StartDate
  - by EndDate
  - by FinalPremium

Response must include:

- TotalCount
- Page
- PageSize
- Items

---

### 4.4 Export Functionality

Administrators must be able to export reports:

- CSV export
- JSON export
- Export respects current filters

Endpoint example:

GET /api/admin/reports/policies-by-country/export?format=csv

---

### 4.5 Audit & Logging

1. Track CreatedAt and UpdatedAt automatically.
2. Log:
   - Policy activation
   - Policy cancellation
   - Broker activation/deactivation
   - Currency deactivation
3. Ensure logs are meaningful and consistent.

---

## 5. Non-Functional Requirements

### 5.1 Security

- JWT authentication
- Role-based authorization
- Brokers cannot access other brokers’ policies
- Proper 401 and 403 handling

---

### 5.2 Scalability

- All reports must run server-side (no in-memory aggregation).
- Pagination must be applied at database level.
- Avoid unnecessary eager loading.

---

### 5.3 Extensibility

- New filters can be added with minimal changes.
- Consider Strategy pattern for export formats.
- Repository methods should remain focused and reusable.

---

### 5.4 Validation

- Validate pagination parameters.
- Validate sorting fields.
- Validate export format.

---

## 6. API Specification (High-Level)

### Authentication

POST /api/auth/login  
POST /api/auth/register  

---

### Broker

GET  /api/brokers/policies?page=&pageSize=&status=&from=&to=&sortBy=  
GET  /api/brokers/policies/{policyId}  

---

### Admin

GET  /api/admin/reports/policies-by-country  
GET  /api/admin/reports/policies-by-country/export?format=csv  

---

## 7. Testing Plan

### 7.1 Unit Tests

- Policy expiration logic
- Authorization logic
- Export logic
- Pagination calculations
- Soft delete behavior

Mock:
- Repositories
- Authentication context
- Logging

---

### 7.2 Integration Tests

- Authentication flow
- Authorization enforcement
- Pagination correctness
- Export endpoints
- Soft delete exclusion

Test DB considerations:

- Clean DB between tests
- Seed data per test
- Isolate test scenarios

---

## 8. Acceptance Criteria

Sprint 3 is complete when:

1. Authentication & Authorization work correctly.
2. Brokers cannot access other brokers’ data.
3. Policies expire correctly.
4. Reports support export.
5. Pagination works correctly.
6. Soft delete is implemented.
7. Logs are consistent.
8. Unit and integration tests pass with ≥ 80% coverage.
