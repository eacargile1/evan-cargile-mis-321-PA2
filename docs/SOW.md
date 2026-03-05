# Statement of Work (SOW)
## Legendary Training Services — Scheduling & Booking System

**Client:** Legendary Training Services (LTS)
**Vendor:** 321 Consulting (UA MIS Program)
**Document Version:** 2.0
**Date:** March 2026

---

## 1. Purpose

This Statement of Work defines the scope, deliverables, timeline, and responsibilities for the design and development of an automated scheduling and booking system for Legendary Training Services (LTS), a pet training services business.

---

## 2. Scope

### 2.1 In Scope

**Core Booking & Scheduling**
- Web-based scheduling and booking application
- Three user roles: Admin, Trainer, Pet Owner
- Support for **classes** (group sessions) and **individual training sessions**
- Minimum enrollment requirement per session (session cancels if not met)
- Realistic class times (morning, midday, evening slots)
- Calendar view of available bookings

**User & Pet Management**
- Full authentication (register/login with email + password, JWT)
- Customer profiles with phone, address, and pet profiles
- Trainer profiles with bio, specialties, and breed preferences
- Pet profiles: name, breed, neutered/spayed status

**Pricing & Packages**
- Trainers set price per session (minimum $30)
- Platform fee: 3% of session price per booking (applied to classes and individual sessions)
- Package deals: bundled sessions at a discounted price
- Revenue tracking: gross, platform fee, trainer payout

**Ratings & Reviews**
- Customers can rate and review trainers after a session (1–5 stars)
- Admin can flag suspicious reviews (admin-only visibility)
- Flagged reviews hidden from public until cleared

**Filtering**
- Filter offerings by breed (trainer specifies allowed breeds per session)
- Filter reports by date range, trainer, session type

**Advertising**
- Discreet sponsored dog food advertising on the site

**Reports (Admin)**
- Bookings report (by date, trainer, type)
- Expected revenue report (gross, platform fee, trainer payout)
- Business report: who I owe (trainer payouts), projected revenue, repeat customers
- Trainer offerings report
- Pet report
- All reports are filterable

### 2.2 Out of Scope (Initial Release)

- Live payment processing (pricing/fee calculations tracked; no payment gateway)
- SMS notifications
- Mobile native apps (web app will be mobile-responsive)
- Integration with external CRM or accounting systems

---

## 3. Deliverables

| # | Deliverable | Description |
|---|-------------|-------------|
| 1 | **Planning Document** | Full planning document (PLANNING.md) |
| 2 | **Statement of Work** | This document |
| 3 | **User Stories** | Role-based user stories in standard format |
| 4 | **Wireframes/Mockups** | UI layouts for key screens |
| 5 | **Requirements Traceability Matrix** | Map requirements to stories, tests, implementation |
| 6 | **Test Plan** | Test strategy, cases, coverage |
| 7 | **Web Application** | Functional scheduling and booking system |

---

## 4. Timeline

| Phase | Duration | Key Milestones |
|-------|----------|----------------|
| Discovery & Design | 1–2 weeks | Requirements finalized, wireframes approved |
| Backend Development | 2–3 weeks | API, auth, database, models operational |
| Frontend Development | 2–3 weeks | All three role UIs complete |
| Reports & Polish | 1 week | All reports, calendar, ads, filtering complete |
| Testing | 1–2 weeks | Test plan executed, defects resolved |
| Launch | 1 week | Deployment, handoff, documentation |

**Total Estimated Duration:** 8–12 weeks

---

## 5. Roles & Responsibilities

### 5.1 Client (LTS)

- Provide business requirements and feedback
- Clarify pricing rules, breed policies, and edge cases
- Review and approve wireframes and deliverables
- Provide access to sample data or domain experts as needed

### 5.2 321 Consulting (Vendor)

- Design and develop the system per agreed scope
- Produce all documentation deliverables
- Conduct testing and fix defects
- Deploy and hand off the application

---

## 6. Acceptance Criteria

- Pet owners can browse, filter by breed, and book sessions
- Trainers can create profiles, set prices, and schedule sessions with min enrollment
- Platform fee calculated at 3% of booking price per session
- Admins can view all bookings, run all report types, and manage flagged reviews
- Authentication enforced across all protected pages
- Mobile-responsive; discreet advertising visible

---

## 7. Assumptions

- LTS will provide timely feedback on deliverables
- Live payment processing is not required for MVP
- Authentication uses email/password (JWT)
- Data hosted in a mutually agreed environment

---

## 8. Risks & Mitigations

| Risk | Mitigation |
|------|------------|
| Scope creep | Changes documented via change request; out-of-scope deferred |
| Timeline slip | Phased delivery; MVP first |
| Min enrollment cancellations | Admin workflow to notify and cancel sessions below minimum |
| Review abuse | Admin flagging workflow; flagged reviews hidden pending review |

---

## 9. Change Management

- Scope changes require written approval from both parties
- Change requests documented with impact on timeline and effort

---

## 10. Sign-Off

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Client (LTS) | | | |
| 321 Consulting | | | |

---

*Document version: 2.0 | Last updated: March 2026*
