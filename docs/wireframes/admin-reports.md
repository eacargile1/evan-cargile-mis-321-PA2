# Admin — Generate & View Reports

## Wireframe

```
+------------------------------------------------------------------+
|  LTS - Admin Portal                      [Dashboard] [Logout]     |
+------------------------------------------------------------------+
|                                                                  |
|  Reports                                                         |
|                                                                  |
|  +------------------------------------------------------------+  |
|  |  Generate Report                                           |  |
|  |                                                             |  |
|  |  Report Type:  [Bookings Report        ▼]                   |  |
|  |                - Bookings by date                           |  |
|  |                - Revenue summary                            |  |
|  |                - Trainer utilization                        |  |
|  |                                                             |  |
|  |  Date Range:   [03/01/2026] to [03/31/2026]                 |  |
|  |  Trainer:      [All Trainers ▼]                             |  |
|  |                                                             |  |
|  |  [Generate]                                                 |  |
|  +------------------------------------------------------------+  |
|                                                                  |
|  Recent Reports                                                  |
|  +------------------------------------------------------------+  |
|  | Bookings | 3/1-3/31/26 | Generated 3/2 2:30 PM  [View]      |  |
|  | Revenue  | 3/1-3/31/26 | Generated 3/1 9:00 AM  [View]      |  |
|  | Utilization | 2/1-2/28/26 | Generated 2/28 5:00 PM [View]  |  |
|  +------------------------------------------------------------+  |
|                                                                  |
+------------------------------------------------------------------+
```

## Report View (after Generate/View)

```
+------------------------------------------------------------------+
|  Bookings Report — March 1–31, 2026                    [Export]  |
+------------------------------------------------------------------+
|                                                                  |
|  Summary: 87 total bookings | 24 classes | 63 individual         |
|                                                                  |
|  By Date                                                        |
|  | Date       | Classes | Individual | Total |                    |
|  |------------|---------|------------|-------|                    |
|  | 3/1/2026   | 2       | 5          | 7     |                    |
|  | 3/2/2026   | 1       | 4          | 5     |                    |
|  | ...        | ...     | ...        | ...   |                    |
|                                                                  |
|  By Trainer                                                     |
|  | Trainer    | Classes | Individual | Total |                    |
|  |------------|---------|------------|-------|                    |
|  | Jane Doe   | 12      | 28         | 40    |                    |
|  | John Smith | 12      | 35         | 47    |                    |
|                                                                  |
+------------------------------------------------------------------+
```

## Elements

- **Generate form**: Report type, date range, trainer filter
- **Recent reports**: List of generated reports with View
- **Report view**: Summary + tables; Export option

## Report Types

1. **Bookings** — By date, trainer, type
2. **Revenue** — If pricing data exists
3. **Trainer utilization** — Booked vs available hours
