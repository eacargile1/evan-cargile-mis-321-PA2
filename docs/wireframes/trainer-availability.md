# Trainer — Schedule Availability

## Wireframe

```
+------------------------------------------------------------------+
|  LTS - Trainer Portal                    [Dashboard] [Logout]    |
+------------------------------------------------------------------+
|                                                                  |
|  My Availability                                                 |
|                                                                  |
|  +------------------------------------------------------------+  |
|  |  Add Availability                                          |  |
|  |                                                             |  |
|  |  Date:    [03/10/2026 ▼]                                    |  |
|  |  Start:   [2:00 PM ▼]    End:   [5:00 PM ▼]                 |  |
|  |  Type:    ( ) Individual  (•) Class slot                    |  |
|  |                                                             |  |
|  |  [Add Slot]                                                 |  |
|  +------------------------------------------------------------+  |
|                                                                  |
|  Upcoming Availability                                          |
|  +------------------------------------------------------------+  |
|  |  Mon 3/10  2:00 PM - 5:00 PM   Individual   [Remove]        |  |
|  |  Tue 3/11  10:00 AM - 12:00 PM Individual   [Remove]        |  |
|  |  Sat 3/15  9:00 AM - 11:00 AM  Class       [Remove]         |  |
|  |  ...                                                        |  |
|  +------------------------------------------------------------+  |
|                                                                  |
|  Calendar View (optional)                                        |
|  [  < March 2026 >  ]                                            |
|  [Grid of days with availability blocks highlighted]            |  |
|                                                                  |
+------------------------------------------------------------------+
```

## Elements

- **Add form**: Date, start time, end time, type (individual/class)
- **Add Slot**: Submit new availability
- **List**: Upcoming slots with Remove option
- **Calendar**: Optional visual view of availability

## User Flow

1. Trainer navigates to My Availability
2. Selects date and time range
3. Clicks Add Slot
4. Slot appears in list and becomes bookable for pet owners
5. Can remove slots to block off time

## Business Rules

- No overlap with existing bookings
- Slots can be removed if no bookings; if booked, may need cancellation flow
