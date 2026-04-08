# Run LTS locally (anyone cloning from GitHub)

Do these in order. No Heroku required.

## 1. Install prerequisites

| Tool | Check |
|------|--------|
| **.NET 9 SDK** | Run `dotnet --version` — should be **9.x**. If not: [Download .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0). |
| **MySQL Server 8** | Running on your machine (default port **3306**). [MySQL Installer](https://dev.mysql.com/downloads/installer/) (Windows) or your OS package manager. |

## 2. Create the database

In **MySQL Workbench** or `mysql` CLI, as a user that can create databases:

```sql
CREATE DATABASE IF NOT EXISTS lts
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;
```

Remember the **username** and **password** you use to connect (often `root` + the password you set at install).

## 3. Local config (required for a typical install)

`dotnet run` uses **Development** mode and loads `appsettings.Development.json` **if that file exists**.

1. Go to folder `src/LTS.Api`.
2. Copy **`appsettings.Development.sample.json`** → **`appsettings.Development.json`** (same folder).
3. Open **`appsettings.Development.json`** and replace `YOUR_MYSQL_ROOT_PASSWORD_HERE` with your real MySQL password.

If the app still cannot connect (common on Windows + MySQL 8), use this connection string shape instead:

```json
"DefaultConnection": "Server=127.0.0.1;Port=3306;Database=lts;User=root;Password=YOUR_PASSWORD;SslMode=None;AllowPublicKeyRetrieval=true;"
```

> **If you skip step 3**, the app falls back to `appsettings.json`, which uses `Password=changeme`. That only works if your MySQL `root` password is literally `changeme`.

## 4. Run the API + website

```bash
cd src/LTS.Api
dotnet run
```

Open the URL shown in the terminal (usually **http://localhost:5258**).

## 5. What you get (all features)

On first successful start, the app **creates tables** and **seeds** sample trainers, sessions, bookings, etc. Then you can:

| Area | URL |
|------|-----|
| Browse / book (guest or owner) | `/` |
| Calendar | `/calendar.html` |
| Register / login | `/login.html` |
| Trainer dashboard | `/trainer.html` (after registering as Trainer) |
| Admin dashboard | `/admin.html` (after registering as Admin) |

Register new accounts from **Create Account** to exercise each role.

## Troubleshooting

| Symptom | Likely fix |
|---------|------------|
| `Access denied` for MySQL | Wrong password in `appsettings.Development.json`, or user/host doesn’t match MySQL (`root`@`localhost`). |
| `Unable to connect` | MySQL service not running; wrong port; firewall. |
| Browser can’t load page | Use `http://` URL from terminal; keep `dotnet run` running. |

## Heroku (optional)

Deploying to the cloud is separate. See [GITHUB_AND_HEROKU.md](GITHUB_AND_HEROKU.md).
