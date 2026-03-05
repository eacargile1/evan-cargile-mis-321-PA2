# Requirements Traceability Matrix
## Legendary Training Services — Scheduling & Booking System

---

## Requirement ID Legend

- **REQ** — Base requirement
- **PO** — Pet Owner user story
- **TR** — Trainer user story
- **AD** — Admin user story
- **TC** — Test case

---

## Traceability Matrix

| Req ID | Requirement | User Stories | Test Case |
|--------|-------------|--------------|-----------|
| REQ-1 | Admins generate reports | AD-2, AD-3, AD-4, AD-5, AD-6 | TC-AD-01, TC-AD-02 |
| REQ-2 | Admins view reports | AD-2 through AD-7 | TC-AD-01 through TC-AD-06 |
| REQ-3 | Admins view training bookings | AD-1 | TC-AD-07 |
| REQ-4 | Trainers create a profile | TR-1, TR-8 | TC-TR-01, TC-TR-02 |
| REQ-5 | Trainers schedule lesson times | TR-2, TR-4 | TC-TR-03, TC-TR-04 |
| REQ-6 | Pet owners view lesson offerings | PO-1, PO-2, PO-3 | TC-PO-01, TC-PO-02 |
| REQ-7 | Pet owners book with available trainers | PO-4, PO-5 | TC-PO-03, TC-PO-04 |
| REQ-8 | System supports classes (group) | PO-1, AD-2 | TC-PO-01 |
| REQ-9 | System supports individual sessions | PO-2, AD-2 | TC-PO-02 |
| REQ-10 | No double-booking | PO-5, TR-2 | TC-PO-04, TC-TR-04 |
| REQ-11 | Authentication (register/login) | PO-7, TR-1 | TC-AUTH-01 through TC-AUTH-04 |
| REQ-12 | Customer profiles (phone, address) | PO-8 | TC-PO-05 |
| REQ-13 | Pet profiles (breed, neutered/spayed) | PO-9 | TC-PO-06 |
| REQ-14 | Filter by breed | PO-10, TR-7 | TC-PO-07 |
| REQ-15 | Customer ratings and reviews | PO-11 | TC-PO-08 |
| REQ-16 | Admin flags bad reviews | AD-8 | TC-AD-08 |
| REQ-17 | Trainer sets session price (min $30) | TR-3 | TC-TR-05 |
| REQ-18 | 3% platform fee per booking | TR-3, AD-3 | TC-AD-03 |
| REQ-19 | Package deals | PO-12, TR-6 | TC-PO-09, TC-TR-06 |
| REQ-20 | Min enrollment; cancel if not met | TR-2 | TC-TR-03 |
| REQ-21 | Trainer adjusts session time | TR-4 | TC-TR-04 |
| REQ-22 | Expected revenue report | AD-3 | TC-AD-03 |
| REQ-23 | Business reports (owe, projected, repeats) | AD-4 | TC-AD-04 |
| REQ-24 | Trainer offerings report | AD-5 | TC-AD-05 |
| REQ-25 | Pet report | AD-6 | TC-AD-06 |
| REQ-26 | All reports are filterable | AD-7 | TC-AD-09 |
| REQ-27 | Calendar view of bookings | PO-15 | TC-PO-10 |
| REQ-28 | Discreet dog food advertising | — | TC-UI-01 |

---

## User Story → Requirement Mapping

| User Story | Requirements |
|------------|-------------|
| PO-1 | REQ-6, REQ-8 |
| PO-2 | REQ-6, REQ-9 |
| PO-3 | REQ-6 |
| PO-4 | REQ-7 |
| PO-5 | REQ-7, REQ-10 |
| PO-7 | REQ-11 |
| PO-8 | REQ-12 |
| PO-9 | REQ-13 |
| PO-10 | REQ-14 |
| PO-11 | REQ-15 |
| PO-12 | REQ-19 |
| PO-15 | REQ-27 |
| TR-1 | REQ-4, REQ-11 |
| TR-2 | REQ-5, REQ-10, REQ-20 |
| TR-3 | REQ-17, REQ-18 |
| TR-4 | REQ-5, REQ-21 |
| TR-6 | REQ-19 |
| TR-7 | REQ-14 |
| AD-1 | REQ-3 |
| AD-2 | REQ-1, REQ-2, REQ-8, REQ-9 |
| AD-3 | REQ-1, REQ-22 |
| AD-4 | REQ-1, REQ-23 |
| AD-5 | REQ-1, REQ-24 |
| AD-6 | REQ-1, REQ-25 |
| AD-7 | REQ-26 |
| AD-8 | REQ-16 |

---

## Implementation Mapping

| Requirement | Implementation Area |
|-------------|---------------------|
| REQ-1, REQ-2 | ReportsController |
| REQ-3 | BookingsController + admin UI |
| REQ-4 | TrainersController + trainer UI |
| REQ-5, REQ-21 | OfferingsController + trainer UI |
| REQ-6 | OfferingsController + browse UI |
| REQ-7 | BookingsController + booking UI |
| REQ-8, REQ-9 | OfferingType enum + data model |
| REQ-10 | Booking validation in BookingsController |
| REQ-11 | AuthController + JWT middleware |
| REQ-12 | User model + UsersController |
| REQ-13 | Pet model + PetsController |
| REQ-14 | Offering.AllowedBreeds + Pet.Breed filter |
| REQ-15 | Review model + ReviewsController |
| REQ-16 | Review.IsFlagged + admin review endpoint |
| REQ-17 | Offering.Price + validation |
| REQ-18 | ReportsController revenue calc (price × 0.97/0.03) |
| REQ-19 | Package model + PackagesController |
| REQ-20 | Offering.MinEnrollment + admin cancel flow |
| REQ-21 | OfferingsController PUT (time adjust) |
| REQ-22 | ReportsController /reports/revenue |
| REQ-23 | ReportsController /reports/business |
| REQ-24 | ReportsController /reports/offerings |
| REQ-25 | ReportsController /reports/pets |
| REQ-26 | Query params on all report endpoints |
| REQ-27 | calendar.html + calendar.js |
| REQ-28 | Discreet ad component in site.css + index.html |

---

*Document version: 2.0 | Last updated: March 2026*
