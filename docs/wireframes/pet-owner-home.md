# Pet Owner — Home / Browse Offerings

## Wireframe

```
+------------------------------------------------------------------+
|  LTS - Legendary Training Services                    [Login]    |
+------------------------------------------------------------------+
|                                                                  |
|  Find Pet Training                                               |
|  [________________________] [Filter ▼] [Search]                 |
|                                                                  |
|  Filters: [All] [Classes] [Individual] [Date ▼] [Trainer ▼]      |
|                                                                  |
+------------------------------------------------------------------+
|  CLASSES                                                         |
+------------------------------------------------------------------+
|  +------------------+  +------------------+  +------------------+|
|  | Puppy Basics     |  | Obedience 101     |  | Agility Intro    ||
|  | Sat 3/8, 10am    |  | Sun 3/9, 2pm     |  | Sat 3/15, 9am   ||
|  | Trainer: Jane D. |  | Trainer: John S.  |  | Trainer: Jane D. ||
|  | 3/8 spots left   |  | 5/8 spots left   |  | 6/8 spots left   ||
|  | [Book Now]       |  | [Book Now]       |  | [Book Now]       ||
|  +------------------+  +------------------+  +------------------+|
+------------------------------------------------------------------+
|  INDIVIDUAL SESSIONS                                             |
+------------------------------------------------------------------+
|  +------------------+  +------------------+  +------------------+|
|  | Jane Doe         |  | John Smith       |  | Jane Doe         ||
|  | Obedience, Puppy |  | Agility, Adult   |  | Obedience        ||
|  | Mon 3/10 2-3pm   |  | Tue 3/11 10-11am |  | Wed 3/12 4-5pm   ||
|  | [Book]           |  | [Book]           |  | [Book]           ||
|  +------------------+  +------------------+  +------------------+|
+------------------------------------------------------------------+
|  Footer: Contact | About | Privacy                              |
+------------------------------------------------------------------+
```

## Elements

- **Header**: Logo, nav, login
- **Search/Filter**: Keyword search, type (class/individual), date, trainer
- **Classes section**: Card grid; each card shows name, date/time, trainer, spots left, Book Now
- **Individual section**: Card grid; each card shows trainer name, specialties, available slot, Book
- **Mobile**: Stack cards vertically; filters collapse into drawer

## User Flow

1. Land on page
2. Optionally filter by type, date, trainer
3. Click Book Now / Book on desired offering
4. Navigate to booking confirmation screen
