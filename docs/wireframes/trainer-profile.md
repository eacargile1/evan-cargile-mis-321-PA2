# Trainer — Profile Create/Edit

## Wireframe

```
+------------------------------------------------------------------+
|  LTS - Trainer Portal                    [Dashboard] [Logout]    |
+------------------------------------------------------------------+
|                                                                  |
|  My Profile                                                      |
|                                                                  |
|  +------------------------------------------------------------+  |
|  |  Photo                                                     |  |
|  |  [    Upload Photo    ]  or  [  placeholder avatar  ]      |  |
|  |                                                             |  |
|  |  Display Name *                                             |  |
|  |  [Jane Doe________________________]                         |  |
|  |                                                             |  |
|  |  Bio *                                                      |  |
|  |  [________________________________________________]         |  |
|  |  [________________________________________________]         |  |
|  |  [________________________________________________]         |  |
|  |                                                             |  |
|  |  Specialties (e.g., Puppy, Obedience, Agility)              |  |
|  |  [Puppy] [Obedience] [x]  [+ Add]                           |  |
|  |                                                             |  |
|  |  Contact Email *                                             |  |
|  |  [jane@example.com________________]                         |  |
|  |                                                             |  |
|  |  [Cancel]                                    [Save Profile]  |  |
|  +------------------------------------------------------------+  |
|                                                                  |
+------------------------------------------------------------------+
```

## Elements

- **Photo**: Upload or placeholder
- **Display Name**: Required
- **Bio**: Multi-line; required
- **Specialties**: Tags (Puppy, Obedience, Agility, etc.)
- **Contact Email**: Required
- **Actions**: Cancel, Save Profile

## User Flow

1. Trainer logs in, navigates to My Profile
2. Fills in or edits profile fields
3. Clicks Save Profile
4. Profile appears in pet owner browse view
