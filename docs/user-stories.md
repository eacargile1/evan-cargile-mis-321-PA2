# User Stories
## Legendary Training Services — Scheduling & Booking System

Format: **As a [role], I want [action] so that [benefit].**

---

## Pet Owner Stories

| ID | Story | Priority |
|----|-------|----------|
| PO-1 | As a pet owner, I want to view available classes so that I can choose a group training option. | P0 |
| PO-2 | As a pet owner, I want to view available individual training sessions so that I can book one-on-one time with a trainer. | P0 |
| PO-3 | As a pet owner, I want to filter offerings by trainer, date, and type so that I can find sessions that fit my schedule. | P0 |
| PO-4 | As a pet owner, I want to book a session with an available trainer so that I can secure a training slot. | P0 |
| PO-5 | As a pet owner, I want to see only available time slots so that I don't attempt to book already-filled sessions. | P0 |
| PO-6 | As a pet owner, I want to use the site on my phone so that I can book while on the go. | P0 |
| PO-7 | As a pet owner, I want to register and log in so that my profile and bookings are saved. | P0 |
| PO-8 | As a pet owner, I want a profile storing my phone number and address so that trainers have my contact info. | P0 |
| PO-9 | As a pet owner, I want to add my pet's profile (name, breed, neutered/spayed status) so the trainer is prepared. | P0 |
| PO-10 | As a pet owner, I want to filter sessions by breed so that I find trainers who work with my type of dog. | P0 |
| PO-11 | As a pet owner, I want to rate and review a trainer after a session so that I can help others make informed decisions. | P0 |
| PO-12 | As a pet owner, I want to purchase a session package so that I get a discounted rate for multiple sessions. | P1 |
| PO-13 | As a pet owner, I want to cancel or reschedule a booking so that I can adjust when my plans change. | P1 |
| PO-14 | As a pet owner, I want to receive a booking confirmation so that I have a record of my reservation. | P1 |
| PO-15 | As a pet owner, I want to view a calendar of available sessions so that I can visualize scheduling options. | P1 |

---

## Trainer Stories

| ID | Story | Priority |
|----|-------|----------|
| TR-1 | As a trainer, I want to create a profile with my bio, specialties, and breed preferences so that pet owners can find the right match. | P0 |
| TR-2 | As a trainer, I want to schedule lesson times with a minimum enrollment requirement so that sessions only run with enough participants. | P0 |
| TR-3 | As a trainer, I want to set a price for each session so that customers know the cost upfront. | P0 |
| TR-4 | As a trainer, I want to adjust the date and time of an existing session so that I can handle schedule changes. | P0 |
| TR-5 | As a trainer, I want to view my upcoming bookings so that I can prepare for sessions. | P0 |
| TR-6 | As a trainer, I want to offer package deals (multiple sessions at a bundled price) so that I can retain customers. | P1 |
| TR-7 | As a trainer, I want to specify which breeds I work with for a given session so that only compatible pets are booked. | P1 |
| TR-8 | As a trainer, I want to update my profile and contact information so that my info stays current. | P1 |
| TR-9 | As a trainer, I want to see my ratings and reviews so that I can improve my services. | P1 |

---

## Admin Stories

| ID | Story | Priority |
|----|-------|----------|
| AD-1 | As an admin, I want to view all training bookings so that I have visibility into scheduled sessions. | P0 |
| AD-2 | As an admin, I want to generate and view a bookings report (by date, trainer, type) so that I can track scheduling. | P0 |
| AD-3 | As an admin, I want an expected revenue report so that I can track projected income. | P0 |
| AD-4 | As an admin, I want a business report (who I owe, projected revenue, repeat customers) so that I can run the business. | P0 |
| AD-5 | As an admin, I want a trainer offerings report so that I know what each trainer is currently offering. | P0 |
| AD-6 | As an admin, I want a pet report so that I can see all registered pets and their details. | P0 |
| AD-7 | As an admin, I want to filter all reports by date, trainer, and type so that I can drill into specific data. | P0 |
| AD-8 | As an admin, I want to view flagged reviews so that I can verify credibility before they appear publicly. | P0 |
| AD-9 | As an admin, I want a trainer utilization report so that I can see how trainers are being used. | P1 |

---

## Acceptance Criteria (Sample)

### PO-4: Book a session with an available trainer

- **Given** I am a logged-in pet owner viewing available slots
- **When** I select a slot and confirm with my pet info
- **Then** the booking is created and the slot capacity decreases by 1
- **And** I see a confirmation message

### TR-2: Schedule available lesson times with minimum enrollment

- **Given** I am a trainer
- **When** I create a session with MinEnrollment=3 and only 1 person books
- **Then** the system flags the session as at-risk of cancellation
- **And** admin can cancel sessions below minimum before the start date

### TR-3: Set session price

- **Given** I am a trainer creating an offering
- **When** I set a price (min $30)
- **Then** the platform fee is calculated as 3% of the price (min platform fee applies)
- **And** my payout = price × 0.97

### AD-3: Expected revenue report

- **Given** I am an admin
- **When** I run the expected revenue report for a date range
- **Then** I see gross revenue (total bookings × price), platform fees (3%), and trainer payouts (97%)

### AD-8: Flag bad reviews (admin only)

- **Given** a customer submits a review
- **When** an admin flags it as suspicious
- **Then** the review is hidden from public view pending admin decision
- **And** the flag is only visible to admins

---

*Document version: 2.0 | Last updated: March 2026*
