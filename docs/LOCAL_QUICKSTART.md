# Run LTS locally (anyone cloning from GitHub)

Do these in order. No Heroku required.

---

## After you pull from GitHub (easy version)

Think of it in three parts: **install tools**, **tell the app how to reach your database**, **start the app**.

1. **Download the project**  
   You already did this: `git clone …` and you have the folder on your computer.

2. **Install two things** (only if you don’t have them yet)  
   - **.NET 9** — this is what runs the app. [Get it here](https://dotnet.microsoft.com/download/dotnet/9.0).  
   - **MySQL** — this is where the app stores data (users, bookings, etc.). [Windows installer](https://dev.mysql.com/downloads/installer/).  
   When you installed MySQL, you chose a **password** for the main user (often named `root`). **Remember that password.**

3. **Make sure MySQL is running**  
   On Windows: open **Services** (`Win + R`, type `services.msc`), find **MySQL** (often **MySQL80**), and make sure it says **Running**.

4. **Create an empty database**  
   Open **MySQL Workbench**, connect with your password, open a **SQL** tab, paste this, click the lightning bolt to run it:

   ```sql
   CREATE DATABASE IF NOT EXISTS lts
     CHARACTER SET utf8mb4
     COLLATE utf8mb4_unicode_ci;
   ```

   That creates a blank database named **`lts`**. The app will fill in the tables the first time it starts.

5. **Add a small config file the repo does not include**  
   GitHub **on purpose** does not store your database password. You add it locally:

   - Go inside the project to the folder **`src`** → **`LTS.Api`**.  
   - Find **`appsettings.Development.sample.json`**.  
   - **Copy** that file and **rename the copy** to **`appsettings.Development.json`** (same folder).  
   - Open **`appsettings.Development.json`** in Notepad (or any editor).  
   - Find **`YOUR_MYSQL_ROOT_PASSWORD_HERE`** and replace it with **the same password you use to log into MySQL Workbench**.  
   - Save the file.

   If the app still says it can’t connect, try changing the connection line to use **`127.0.0.1`** and the extra bits shown in **step 3** of the detailed section below.

6. **Start the app**  
   Open **PowerShell** or **Command Prompt**, go to the **`LTS.Api`** folder, and run:

   ```bash
   cd src/LTS.Api
   dotnet run
   ```

   Wait until you see a line that says the app is **listening** and gives a link like **`http://localhost:5258`**.

7. **Open the site in your browser**  
   Use that link. You should see the training site. Keep the terminal window open while you test; closing it stops the app.

**That’s it.** The first successful run creates the tables and sample data. To see trainer or admin screens, use **Create account** on the login page and pick **Trainer** or **Admin**.

---

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
