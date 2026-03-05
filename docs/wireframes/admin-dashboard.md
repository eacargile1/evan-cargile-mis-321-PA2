# Admin — Dashboard / View Bookings

## Wireframe

```
+------------------------------------------------------------------+
|  LTS - Admin Portal                      [Reports] [Logout]     |
+------------------------------------------------------------------+
|                                                                  |
|  Training Bookings                                               |
|                                                                  |
|  Filters: [All] [Classes] [Individual] [Date range ▼] [Trainer ▼]|
|  [Apply]                                                         |
|                                                                  |
|  +------------------------------------------------------------+  |
|  | Date       | Time     | Type      | Trainer   | Customer    |  |
|  |------------|----------|-----------|-----------|-------------|  |
|  | 3/10/2026  | 2:00 PM  | Individual| Jane Doe  | john@x.com  |  |
|  | 3/10/2026  | 3:00 PM  | Individual| Jane Doe  | sarah@y.com  |  |
|  | 3/11/2026  | 10:00 AM | Individual| John Smith| mike@z.com   |  |
|  | 3/15/2026  | 9:00 AM  | Class     | Jane Doe  | 6 attendees |  |
|  | ...        | ...      | ...       | ...       | ...         |  |
|  +------------------------------------------------------------+  |
|                                                                  |
|  [< Prev]  Page 1 of 5  [Next >]                                 |
|                                                                  |
|  Summary: 24 bookings this week | 8 classes | 16 individual     |
|                                                                  |
+------------------------------------------------------------------+
```

## Elements

- **Filters**: Type (class/individual), date range, trainer
- **Table**: Date, time, type, trainer, customer (or attendee count for classes)
- **Pagination**: For large result sets
- **Summary**: Quick stats (bookings this week, breakdown by type)

## User Flow

1. Admin logs in, lands on dashboard
2. Optionally applies filters
3. Reviews booking list
4. Can export or navigate to Reports for deeper analysis
