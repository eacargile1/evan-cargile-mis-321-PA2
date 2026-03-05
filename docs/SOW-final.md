# Statement of Work (SOW)
## Legendary Training Services — Scheduling & Booking System

---

| | |
|---|---|
| **Client** | Legendary Training Services (LTS) |
| **Vendor** | 321 Consulting |
| **Project Name** | LTS Web-Based Scheduling & Booking System |
| **Document Version** | 2.0 |
| **Date Prepared** | March 2026 |
| **Prepared By** | 321 Consulting — UA MIS Program |

---

## 1. Project Overview

### 1.1 Background

Legendary Training Services (LTS) is a professional pet training business offering group obedience classes and individualized training sessions for dogs of all breeds and skill levels. Prior to this engagement, LTS managed scheduling and bookings manually, resulting in inefficiencies in session management, payment tracking, and customer communication.

321 Consulting was engaged to design and deliver a full-stack web-based platform to automate LTS's scheduling, booking, and reporting operations, enabling trainers to manage their availability and packages while allowing pet owners to discover and book training services online.

### 1.2 Purpose

This Statement of Work formally defines the scope of services, project deliverables, timeline, roles and responsibilities, and acceptance criteria for the LTS Scheduling & Booking System. It serves as the authoritative agreement between LTS and 321 Consulting for this project engagement.

---

## 2. Scope of Work

### 2.1 In Scope

#### 2.1.1 Platform & Technology
- Full-stack web application built with **ASP.NET Core 9 Web API** (backend) and **Vanilla JavaScript / HTML / CSS** (frontend)
- **Entity Framework Core** with **SQLite** database for data persistence
- **JWT Bearer token** authentication and **BCrypt** password hashing
- Mobile-responsive user interface accessible in all modern browsers
- REST API architecture with clearly defined endpoints for all features

#### 2.1.2 User Roles & Authentication
- Three distinct user roles: **Admin**, **Trainer**, and **Pet Owner**
- Registration and login for all roles via email and password
- Role-based access control enforced on all protected pages and API endpoints
- Automatic redirection to the appropriate portal upon successful login

#### 2.1.3 Booking & Scheduling
- Support for two session types: **Group Classes** and **Individual Sessions**
- Trainers may specify a minimum enrollment threshold per session; sessions below minimum may be cancelled
- Session booking form collects: customer name, email, phone number, and optional pet name
- Booking reduces available capacity by one; fully booked sessions are marked accordingly
- Sessions may not be booked past their maximum capacity

#### 2.1.4 Trainer Functionality
- Trainers may create and update a public profile including display name, bio, specialties, and breed preferences
- Trainers may create sessions with: title, type, date, start/end time, price (minimum $30), capacity, minimum enrollment, and optional breed restrictions
- Trainers may edit session date/time after creation
- Trainers may create and offer bundled training packages at a discounted total price
- Trainers may view all customer ratings and reviews submitted for their sessions

#### 2.1.5 Pet Owner Functionality
- Pet owners may browse all available upcoming sessions on the Browse Sessions page
- Filter functionality supports: session type (Class / Individual), breed, and date range
- Pet owners may view a monthly calendar view of all available sessions with color-coded indicators
- Pet owners may purchase training packages listed by trainers
- Pet owners may rate and review trainers (1–5 stars + comment) after attending a session

#### 2.1.6 Pricing & Revenue Model
- Trainers set their own price per session (minimum $30 enforced)
- Platform fee of **3%** applied to each booking automatically
- Trainer payout calculated as **97%** of the booking price
- Revenue tracking covers: gross revenue, platform fee total, and trainer payout per trainer

#### 2.1.7 Reviews & Moderation
- Customers may submit one review per trainer
- Admins may flag reviews deemed suspicious or abusive
- Flagged reviews are hidden from all public and trainer-facing views until cleared by an admin

#### 2.1.8 Advertising
- Sponsored pet product advertising (dog food brands) displayed at the bottom of the home page
- Advertising is non-intrusive and clearly labeled as "Sponsored"

#### 2.1.9 Admin Reporting
The admin portal provides the following reports, each with filtering capabilities:

| Report | Description |
|---|---|
| **Bookings Report** | All bookings with filtering by date range and session type |
| **Revenue Report** | Gross revenue, platform fee, and trainer payouts for a given date range |
| **Business Report** | Trainer payouts owed, projected revenue from future bookings, and repeat customer list |
| **Offerings Report** | All trainer sessions with at-risk flag for sessions below minimum enrollment |
| **Pet Report** | All registered pets filterable by breed |
| **Flagged Reviews** | Admin interface to view, flag, and unflag customer reviews |

### 2.2 Out of Scope

The following items are explicitly excluded from this engagement and may be addressed in a future phase:

- Live payment processing or integration with a payment gateway (e.g., Stripe, PayPal)
- SMS or email notifications for bookings, cancellations, or reminders
- Native mobile applications (iOS / Android)
- Integration with external CRM, accounting, or point-of-sale systems
- Automated session cancellation workflow when minimum enrollment is not met

---

## 3. Deliverables

| # | Deliverable | Description | Format |
|---|---|---|---|
| 1 | **Statement of Work** | This document; defines scope, timeline, and responsibilities | Markdown / PDF |
| 2 | **User Stories** | Role-based user stories for Admin, Trainer, and Pet Owner with acceptance criteria | Markdown |
| 3 | **Requirements Traceability Matrix** | Maps each requirement to a user story, test case, and implementation feature | Markdown / Spreadsheet |
| 4 | **Test Plan** | Testing strategy, test levels, and sample test cases | Markdown |
| 5 | **Test Logs** | Executed test case records with steps, expected results, actual results, and pass/fail status | Markdown / Spreadsheet |
| 6 | **Web Application** | Fully functional scheduling and booking web application | ASP.NET Core + HTML/CSS/JS |

---

## 4. Project Timeline

| Phase | Key Activities | Duration |
|---|---|---|
| **Phase 1 — Discovery & Design** | Requirements gathering, user story creation, wireframe design, SOW finalization | 1–2 weeks |
| **Phase 2 — Backend Development** | Database design, EF Core models, REST API endpoints, authentication, seeding | 2–3 weeks |
| **Phase 3 — Frontend Development** | All three role portals (Admin, Trainer, Pet Owner), navigation, browse/filter, calendar | 2–3 weeks |
| **Phase 4 — Reports & Polish** | Admin reports, review system, advertising, UI refinement, responsive design | 1 week |
| **Phase 5 — Testing** | Test plan execution, defect identification and resolution, test log documentation | 1–2 weeks |
| **Phase 6 — Delivery & Handoff** | Final documentation, deployment, client walkthrough | 1 week |

**Total Estimated Duration:** 8–12 weeks

---

## 5. Roles & Responsibilities

### 5.1 Client — Legendary Training Services (LTS)

| Responsibility |
|---|
| Provide and approve business requirements prior to development |
| Clarify pricing rules, breed policies, enrollment minimums, and edge cases |
| Review and approve wireframes and major deliverables |
| Provide timely feedback within 3 business days of deliverable submission |
| Accept or reject the final application based on the agreed acceptance criteria |

### 5.2 Vendor — 321 Consulting

| Responsibility |
|---|
| Design and develop the application in accordance with the agreed scope |
| Produce all documentation deliverables listed in Section 3 |
| Execute the test plan and log all results |
| Resolve all defects identified during testing prior to handoff |
| Deploy the application and provide a handoff walkthrough to the LTS team |

---

## 6. Acceptance Criteria

The project will be considered complete and acceptable for handoff when all of the following conditions are verified:

1. **Authentication** — Users can register and log in as Pet Owner, Trainer, or Admin; JWT tokens enforce role-based access control; all protected pages redirect unauthorized users to the login page.

2. **Browse & Book** — Pet owners can browse all available sessions, filter by type, breed, and date range, and successfully complete a booking; fully booked sessions cannot be booked; capacity decrements correctly.

3. **Trainer Portal** — Trainers can create and update their profile; create sessions with a minimum price of $30; edit session times; create packages; view their reviews and average rating.

4. **Platform Fee** — The 3% platform fee and 97% trainer payout are calculated correctly on all bookings and reflected in revenue reports.

5. **Admin Reports** — All six admin reports render correctly with accurate data and support the specified filtering options.

6. **Reviews** — Pet owners can submit one review per trainer; admin can flag and unflag reviews; flagged reviews are hidden from non-admin views.

7. **Calendar** — The session calendar displays all upcoming sessions in a monthly view with navigation; clicking a day shows session details for that date.

8. **Responsive UI** — The application is functional and visually acceptable on both desktop and mobile viewports.

9. **Advertising** — Sponsored advertising bar is visible on the home page and clearly labeled.

10. **Test Documentation** — Test logs demonstrate at least 35 executed test cases with a Pass rate of 95% or higher; all failing cases have documented resolutions.

---

## 7. Assumptions

- LTS will provide timely written feedback (within 3 business days) on submitted deliverables.
- Live payment processing is not required for the MVP; pricing and fee calculations are tracked in the database only.
- Authentication is email and password-based only; OAuth/SSO is not required.
- The application will be hosted in a development/academic environment; production-grade cloud infrastructure is out of scope.
- Browser support targets the latest versions of Chrome, Firefox, Edge, and Safari.

---

## 8. Risks & Mitigations

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Scope creep expands timeline | Medium | High | All scope changes documented via change request; out-of-scope items deferred to Phase 2 |
| Timeline delays due to integration issues | Low | Medium | Backend and frontend developed in parallel with agreed API contracts; phased MVP delivery |
| Minimum enrollment cancellations confusion | Medium | Low | Admin portal shows at-risk sessions report; admin workflow to notify and cancel below-threshold sessions |
| Review abuse or fake ratings | Low | Medium | Admin flagging workflow; flagged reviews hidden pending review; admin-only unflag capability |
| Data loss in development | Low | High | SQLite database seeded with realistic test data; application re-seedable from `DataSeeder.cs` |

---

## 9. Change Management

Any request to modify the scope, deliverables, or timeline after SOW approval must follow this process:

1. The requesting party submits a written change request describing the proposed change and justification.
2. 321 Consulting assesses the impact on schedule, effort, and deliverables within 5 business days.
3. Both parties must sign off on the change before work on the change begins.
4. Approved changes are appended to this document as a versioned addendum.

---

## 10. Sign-Off

By signing below, both parties agree to the scope, deliverables, timeline, and terms defined in this Statement of Work.

| Role | Name | Signature | Date |
|---|---|---|---|
| Client — Legendary Training Services | | | |
| Vendor — 321 Consulting | | | |

---

*Document Version: 2.0 | Project: LTS Scheduling & Booking System | Prepared: March 2026 | Team: 321 Consulting — UA MIS Program*
