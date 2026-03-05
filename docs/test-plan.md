# Test Plan
## Legendary Training Services — Scheduling & Booking System

---

## 1. Test Strategy

### Scope

- **Unit tests**: Business logic, validation, data access
- **Integration tests**: API endpoints, database operations
- **End-to-end (E2E) tests**: Critical user flows across roles

### Test Levels

| Level | Tool (Proposed) | Coverage Target |
|-------|-----------------|-----------------|
| Unit | xUnit / NUnit | Core logic, validation |
| Integration | ASP.NET Core TestHost | API contracts |
| E2E | Playwright / Selenium | Key user journeys |

---

## 2. Test Cases

### 2.1 Pet Owner Tests

| TC ID | Description | Type | Precondition | Steps | Expected Result |
|-------|-------------|------|--------------|-------|-----------------|
| TC-PO-01 | View classes | E2E | Offerings exist | Navigate to home, filter Classes | Classes displayed |
| TC-PO-02 | View individual sessions | E2E | Offerings exist | Navigate to home, filter Individual | Individual sessions displayed |
| TC-PO-03 | Book a session | E2E | Slot available | Select slot, enter info, confirm | Booking created; slot no longer available |
| TC-PO-04 | Cannot book occupied slot | Integration | Slot booked | Attempt booking same slot | Error; booking rejected |
| TC-PO-05 | Filter by trainer | E2E | Multiple trainers | Filter by trainer X | Only X's offerings shown |
| TC-PO-06 | Filter by date | E2E | Offerings across dates | Filter by date range | Only offerings in range shown |
| TC-PO-07 | Mobile responsive | E2E | — | Resize to mobile viewport | Layout adapts; usable on small screen |

### 2.2 Trainer Tests

| TC ID | Description | Type | Precondition | Steps | Expected Result |
|-------|-------------|------|--------------|-------|-----------------|
| TC-TR-01 | Create profile | E2E | Trainer account | Fill profile, save | Profile saved; visible to pet owners |
| TC-TR-02 | Edit profile | E2E | Profile exists | Edit bio, save | Changes persisted |
| TC-TR-03 | Add availability | E2E | Trainer logged in | Add date/time range, submit | Slot appears; bookable |
| TC-TR-04 | Cannot double-book | Integration | Slot exists | Add overlapping availability | Rejected or merged per rules |
| TC-TR-05 | View my bookings | E2E | Bookings exist | Login as trainer, view dashboard | Own bookings listed |

### 2.3 Admin Tests

| TC ID | Description | Type | Precondition | Steps | Expected Result |
|-------|-------------|------|--------------|-------|-----------------|
| TC-AD-01 | Generate report | E2E | Admin logged in, data exists | Select report type, date range, generate | Report generated |
| TC-AD-02 | View report | E2E | Report exists | Open report from list | Report displays correctly |
| TC-AD-03 | View all bookings | E2E | Bookings exist | Login as admin, view dashboard | All bookings listed |
| TC-AD-04 | Filter bookings by date | E2E | Bookings exist | Apply date filter | Filtered results |
| TC-AD-05 | Filter bookings by trainer | E2E | Bookings exist | Apply trainer filter | Filtered results |

### 2.4 Security & Auth Tests

| TC ID | Description | Type | Precondition | Steps | Expected Result |
|-------|-------------|------|--------------|-------|-----------------|
| TC-AUTH-01 | Unauthenticated access to admin | Integration | — | Request admin endpoint without auth | 401/403 |
| TC-AUTH-02 | Trainer cannot access admin | Integration | Trainer logged in | Request admin endpoint | 403 |
| TC-AUTH-03 | Pet owner cannot access trainer | Integration | Pet owner logged in | Request trainer endpoint | 403 |

---

## 3. Test Data Requirements

- **Trainers**: 2+ with profiles and availability
- **Offerings**: Classes and individual sessions across multiple dates
- **Bookings**: Mix of booked and available slots
- **Users**: Admin, Trainer, Pet Owner accounts

---

## 4. Test Environment

- **Dev**: Local SQLite / SQL Server; localhost
- **Test**: Isolated DB; seeded test data
- **E2E**: Staging or dedicated test instance

---

## 5. Entry/Exit Criteria

### Entry

- Build passes
- Test data seeded
- Environment configured

### Exit

- All P0 test cases pass
- No critical or high defects open
- Regression suite pass

---

## 6. Defect Severity

| Severity | Definition |
|----------|------------|
| Critical | System unusable; data loss; security breach |
| High | Major feature broken; no workaround |
| Medium | Feature impaired; workaround exists |
| Low | Minor UI/UX; cosmetic |

---

## 7. Traceability to Requirements

See [requirements-traceability.md](requirements-traceability.md) for mapping of test cases to requirements and user stories.

---

*Document version: 1.0 | Last updated: March 2026*
