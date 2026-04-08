# LTS — Legendary Training Services
### Scheduling & Booking System

> Built by Evan Cargile | UA MIS Program | Spring 2026

A full-stack web application for automating pet training scheduling, booking, and business reporting. Supports three user roles — **Pet Owner**, **Trainer**, and **Admin** — each with a dedicated experience.

---

---

# ✅ REQUIREMENTS AND HOW THEY ARE MET

---

## Requirement 1 — Admins can generate/view reports and view training bookings

**How to access:**
1. Run the app (`dotnet run` from `src/LTS.Api`)
2. Open `http://localhost:5258/login.html`
3. Click **Create Account**, set Role to **Admin**, fill in name/email/password, click **Register**
4. You will be automatically logged in and redirected to `http://localhost:5258/admin.html`

**What you will see:**
- **Bookings tab** — a full table of every training booking across all trainers, filterable by date range and session type
- **Revenue tab** — gross revenue, platform fees (3%), and trainer payouts broken down by trainer
- **Business tab** — who the platform owes money to, projected future revenue, and repeat customer list
- **Offerings Report tab** — all scheduled sessions with at-risk flagging (sessions below minimum enrollment highlighted in yellow)
- **Pets tab** — all registered pets with breed breakdown
- **Flagged Reviews tab** — suspicious reviews for admin moderation

---

## Requirement 2 — Trainers can create a profile and schedule lesson times

**How to access:**
1. Open `http://localhost:5258/login.html`
2. Click **Create Account**, set Role to **Trainer**, fill in name/email/password, click **Register**
3. You will be redirected to `http://localhost:5258/trainer.html`

**What you will see:**
- **Profile tab** — set your display name, bio, specialties, and breed specialties (visible to pet owners on the browse page)
- **My Sessions tab** — create new class or individual sessions by entering date, time, price, capacity, minimum enrollment, and allowed breeds; view and edit your existing sessions
- **Packages tab** — bundle multiple sessions into a discounted package for pet owners to purchase
- **Reviews tab** — view all customer reviews and your average star rating

---

## Requirement 3 — Pet owners can view lesson offerings and book with available trainers

**How to access:**
1. Open `http://localhost:5258` (the home page — no login required to browse)
2. To book, either browse as a guest (enter your info at checkout) or create a **Pet Owner** account at `/login.html`

**What you will see:**
- A card grid of all upcoming classes and individual sessions from all trainers
- Each card shows trainer name, date/time, price, spots remaining, and star rating
- Filter by breed, session type, or date range
- Click **Book Now** on any card, fill in your name/email/phone, and confirm — your spot is reserved instantly
- **Calendar view** at `/calendar.html` — a full month calendar showing sessions by day with color-coded dots
- **Packages** section below the main grid — bundled session deals from trainers

---

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Tech Stack](#tech-stack)
3. [Project Structure](#project-structure)
4. [Pages Overview](#pages-overview)
5. [Pet Owner Features](#pet-owner-features)
6. [Trainer Features](#trainer-features)
7. [Admin Features](#admin-features)
8. [Authentication](#authentication)
9. [Pricing & Revenue Model](#pricing--revenue-model)
10. [REST API Reference](#rest-api-reference)
11. [Database Models](#database-models)
12. [Documentation](#documentation)

---

## Getting Started

**Prerequisites:** .NET 9 SDK

```bash
# Clone/open the project
cd src/LTS.Api

# Run the app (auto-creates tables in MySQL and seeds data)
dotnet run
```

Open your browser to the URL shown (e.g. `http://localhost:5258`).

> On first run the database is created and seeded with two trainers, sample pets, realistic class times, packages, and bookings.

**Local MySQL:** Copy `src/LTS.Api/appsettings.Development.sample.json` to `appsettings.Development.json`, set your DB password, and ensure database `lts` exists. **GitHub vs Heroku / collaborators:** see [docs/GITHUB_AND_HEROKU.md](docs/GITHUB_AND_HEROKU.md).

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | C# / ASP.NET Core 9 Web API |
| ORM | Entity Framework Core (Pomelo) |
| Auth | JWT Bearer tokens + BCrypt password hashing |
| Frontend | Vanilla JavaScript + CSS (no framework) |
| Database | MySQL (Heroku / JawsDB) |

---

## Project Structure

```
LTS/
├── docs/
│   ├── PLANNING.md                    # Full planning document
│   ├── SOW.md                         # Statement of Work
│   ├── user-stories.md                # 24 user stories across 3 roles
│   ├── requirements-traceability.md   # Requirements → stories → tests
│   ├── test-plan.md                   # Test strategy + 20 test cases
│   └── wireframes/                    # ASCII wireframes for 6 screens
├── src/
│   ├── LTS.Core/           # Domain models + repository interfaces
│   ├── LTS.Infrastructure/ # EF Core DbContext + repository implementations
│   └── LTS.Api/
│       ├── Controllers/    # REST API controllers
│       ├── wwwroot/        # Frontend (HTML, CSS, JS)
│       └── Program.cs      # App startup + DI
└── LTS.sln
```

---

## Pages Overview

| URL | Who Uses It | Purpose |
|-----|-------------|---------|
| `/` | Pet Owners | Browse and book sessions; view packages |
| `/calendar.html` | Anyone | Month calendar view of all available sessions |
| `/login.html` | All users | Sign in or create an account |
| `/trainer.html` | Trainers | Manage profile, sessions, packages, reviews |
| `/admin.html` | Admins | View bookings and run all business reports |

---

## Pet Owner Features

### Browse Offerings (`/`)

- View all upcoming **classes** (group) and **individual sessions** in a card grid
- Each card shows: title, trainer, date/time, price, spots left, minimum enrollment, and average star rating
- Sessions below minimum enrollment show an **"⚠ Low enrollment"** badge
- **Breed filter** — type a breed (e.g. "Labrador") to show only sessions that accept that breed
- **Type filter** — filter to Classes only or Individual only
- **Date range filter** — narrow results to a specific window
- Fully booked sessions show a disabled "Fully Booked" button

### Booking a Session

1. Click **Book Now** on any available session
2. Fill in name, email, phone, and optional pet name
3. Click **Confirm Booking** — spot is reserved immediately

### Session Packages

- Browse bundled packages (e.g. "5 classes for $200") below the individual offerings
- Each package shows price per session vs. total, and trainer name
- Click **Book Package** to purchase; contact the trainer to schedule individual sessions

### Calendar View (`/calendar.html`)

- Full month calendar with navigation (prev/next month)
- Each day shows colored session dots: **green** = Individual, **blue** = Class, **grey** = Full
- Click any day to see a detail panel listing every session that day with time, trainer, price, and spots left

### Discreet Advertising

- A subtle "Sponsored" bar appears at the top of the browse page
- Features dog food brands (Royal Canin, Hill's Science Diet) with small logos and one-line descriptions
- Clearly labeled "Sponsored" — unobtrusive, not pop-up based

---

## Trainer Features

All trainer functions are accessible from `/trainer.html`, organized into four tabs.

### Profile Tab

- Set **display name**, **bio**, and **specialties** (e.g. Puppy, Obedience, Agility)
- Set **breed specialties** — comma-separated list of breeds you work with (e.g. `Labrador,Beagle,Golden Retriever`)
- Changes save immediately and appear on the public browse page

### My Sessions Tab

- Add a new session by selecting:
  - **Date, start time, end time**
  - **Type** — Class or Individual
  - **Title**
  - **Price** (minimum $30 enforced)
  - **Minimum enrollment** — session is flagged as "at risk" if bookings fall below this count before the start date
  - **Capacity** — max number of bookings
  - **Allowed breeds** — leave blank to accept all breeds; enter comma-separated breeds to restrict (e.g. `Border Collie,Australian Shepherd`)
- Your upcoming sessions are listed showing: title, date/time, price, booked/capacity count, and at-risk status
- Click **Edit Time** on any session to adjust the date and start/end time (trainer time adjustment)

### Packages Tab

- Create a **session package**: title, description, number of sessions, and total price
- Example: "Private Training Bundle — 4 sessions for $300"
- Packages appear on the public browse page for pet owners to purchase
- View how many customers have purchased each package

### Reviews Tab

- See all public reviews left by customers after completed sessions
- Shows star rating, customer name, comment, and date
- Displays your overall average rating across all reviews

---

## Admin Features

All admin functions are accessible from `/admin.html`, organized into six tabs.

### Bookings Tab

- View a table of **all training bookings** across all trainers
- Columns: Date, Time, Type, Session title, Trainer, Customer name/email, Pet, Price
- Filter by **date range** and **session type** (Class / Individual)

### Revenue Tab (Expected Revenue Report)

- Enter a date range and click **Generate**
- Summary row: **Gross Revenue**, **Platform Fee (3%)**, **Trainer Payouts (97%)**
- Breakdown table by trainer showing: bookings, gross, platform fee, payout
- Example: a $100 session → $3.00 platform fee → $97.00 to trainer

### Business Tab

- **Who I Owe** — table of trainer payouts: shows each trainer's total gross revenue and the exact dollar amount owed to them
- **Projected Revenue** — counts upcoming (future) bookings and calculates expected gross, fee, and net for the platform
- **Repeat Customers** — lists customers with more than one booking, sorted by booking count, showing total spent

### Offerings Report Tab

- Full list of all trainer offerings with: title, trainer, type, date/time, price, booked/capacity, minimum enrollment, and status
- Rows highlighted in **yellow** for sessions that are "at risk" (bookings below minimum enrollment)
- Summary: total offerings, at-risk count, total projected gross, total platform fees

### Pets Tab

- Table of all registered pets: name, breed, neutered/spayed status, owner name, and booking count
- **Filter by breed** to see all pets of a specific breed
- Useful for understanding the customer base and trainer demand by breed

### Flagged Reviews Tab (Admin Only)

- Lists all customer reviews that have been flagged as suspicious
- Shows: trainer name, customer, rating, comment, and the flag reason
- Admin can **Unflag** a review to restore it to public visibility
- Flagged reviews are hidden from public and trainer views until cleared

---

## Authentication

- **Register** at `/login.html` — choose role (Pet Owner or Trainer), enter name, email, password, phone, address
- **Sign In** — returns a JWT token stored in `localStorage`
- Token is sent as a `Bearer` header on protected API calls
- Passwords are hashed with **BCrypt** (never stored in plain text)
- Role is embedded in the token: `Admin`, `Trainer`, or `PetOwner`
- After login, users are redirected to their role's home page

> **Seeded dev accounts** (require re-seeding with real hashed passwords for login to work — register a new account for testing):
> - `admin@lts.local` / `jane@lts.local` / `john@lts.local` / `sarah@example.com`

---

## Pricing & Revenue Model

| Rule | Detail |
|------|--------|
| Trainer sets price | Per session (classes and individual) |
| Minimum price | **$30** — enforced at API level |
| Platform fee | **3%** of session price per booking |
| Trainer payout | **97%** of session price per booking |
| Packages | Trainer sets total bundle price; platform fee applies per session redeemed |

**Example:**
- Trainer sets price: $85
- Customer books → LTS earns: $2.55 (3%)
- Trainer payout: $82.45 (97%)

---

## REST API Reference

### Auth

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login, returns JWT |

### Trainers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/trainers` | List all trainers (filter: `?breed=Labrador`) |
| GET | `/api/trainers/{id}` | Trainer detail with reviews + avg rating |
| POST | `/api/trainers` | Create trainer profile |
| PUT | `/api/trainers/{id}` | Update trainer profile |

### Offerings

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/offerings` | List active offerings (filter: `?type=`, `?breed=`, `?from=`, `?to=`) |
| GET | `/api/offerings/{id}` | Single offering detail |
| GET | `/api/offerings/trainer/{id}` | All offerings by trainer |
| POST | `/api/offerings` | Create offering (min price $30 enforced) |
| PUT | `/api/offerings/{id}` | Update offering (including time adjustment) |
| PATCH | `/api/offerings/{id}/cancel` | Cancel offering |

### Bookings

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookings` | All bookings (filter: `?from=`, `?to=`, `?trainerId=`) |
| GET | `/api/bookings/{id}` | Single booking |
| POST | `/api/bookings` | Create booking (validates capacity) |
| PATCH | `/api/bookings/{id}/cancel` | Cancel booking |

### Packages

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/packages` | All active packages |
| GET | `/api/packages/trainer/{id}` | Trainer's packages |
| POST | `/api/packages` | Create package |
| POST | `/api/packages/{id}/book` | Purchase a package |

### Pets

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/pets` | All pets (admin) |
| GET | `/api/pets/owner/{ownerId}` | Pets by owner |
| POST | `/api/pets` | Add pet profile |
| PUT | `/api/pets/{id}` | Update pet profile |

### Reviews

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/reviews/trainer/{id}` | Public reviews for a trainer |
| GET | `/api/reviews/flagged` | Flagged reviews (admin only) |
| POST | `/api/reviews` | Submit a review (rating 1–5) |
| POST | `/api/reviews/{id}/flag` | Flag a review (admin) |
| POST | `/api/reviews/{id}/unflag` | Unflag a review (admin) |

### Reports (Admin)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/reports/bookings` | Bookings report (filter: `?from=`, `?to=`, `?trainerId=`, `?type=`) |
| GET | `/api/reports/revenue` | Revenue report with platform fee breakdown |
| GET | `/api/reports/business` | Business report: payouts, projected revenue, repeat customers |
| GET | `/api/reports/offerings` | Trainer offerings report with at-risk flagging |
| GET | `/api/reports/pets` | Pet report (filter: `?breed=`) |

---

## Database Models

| Model | Key Fields |
|-------|------------|
| `User` | Email, PasswordHash, Role, FirstName, LastName, Phone, Address |
| `TrainerProfile` | DisplayName, Bio, Specialties, BreedSpecialties |
| `Pet` | Name, Breed, IsNeutered, IsSpayed, BirthDate, OwnerId |
| `Offering` | Type, Title, Price, Capacity, MinEnrollment, AllowedBreeds, Status |
| `Booking` | OfferingId, CustomerName/Email/Phone, PetId, Status |
| `Package` | Title, NumSessions, TotalPrice, TrainerProfileId |
| `PackageBooking` | PackageId, CustomerName/Email, SessionsUsed, Status |
| `Review` | TrainerProfileId, BookingId, Rating (1–5), Comment, IsFlagged, IsHidden |

---

## Documentation

All project documentation lives in `docs/`:

| File | Contents |
|------|----------|
| `docs/PLANNING.md` | Full planning document with architecture, goals, features |
| `docs/SOW.md` | Statement of Work — scope, timeline, sign-off |
| `docs/user-stories.md` | 24 role-based user stories with acceptance criteria |
| `docs/requirements-traceability.md` | 28 requirements mapped to stories, tests, and implementation |
| `docs/test-plan.md` | Test strategy, 20+ test cases, entry/exit criteria |
| `docs/wireframes/` | ASCII wireframes for all 6 key screens |

---

*Evan Cargile | UA MIS | Spring 2026*
