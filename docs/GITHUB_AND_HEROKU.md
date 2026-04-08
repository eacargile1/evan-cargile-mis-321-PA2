# GitHub vs Heroku (plain English)

## Two separate systems

| | **GitHub** | **Heroku** |
|---|------------|------------|
| **What it is** | Stores your **source code** (the project files). | Runs your app **in the cloud** on their servers. |
| **Runs on** | Nothing “runs” there—people **download** the repo. | The live website/API runs on **Heroku’s** computers, not on your laptop. |
| **Database** | Not included. | You attach add-ons (e.g. **JawsDB MySQL**); Heroku injects `JAWSDB_URL`. |

Cloning from GitHub gives you **files**. Deploying to Heroku **builds** those files into a container and **hosts** the site at `https://your-app.herokuapp.com`.

Nobody “runs Heroku on their device” in the sense of hosting the public site locally—the **Heroku CLI** on a machine is only used to **push** code and **configure** the cloud app.

---

## What someone needs to run the project **on their own computer**

1. **.NET 9 SDK**
2. **MySQL** running locally (or any MySQL they can reach)
3. Clone the repo from GitHub
4. Copy `src/LTS.Api/appsettings.Development.sample.json` → `appsettings.Development.json` (this file is **gitignored** on purpose) and set `Password=` to their MySQL user
5. Create a database named `lts` (or match the connection string)
6. From `src/LTS.Api`: `dotnet run` → open `http://localhost:5258`

Secrets and machine-specific settings stay **out of GitHub**; the **sample** file shows the shape only.

---

## What someone needs to deploy **their own** Heroku copy

Each person (or team) typically has **their own** Heroku app name and **their own** JawsDB instance—unless you share one Heroku account/app.

1. [Heroku account](https://signup.heroku.com/) + [Heroku CLI](https://devcenter.heroku.com/articles/heroku-cli)
2. `heroku login` (this fixes Git pushes to `git.heroku.com`—**not** your GitHub password)
3. Create an app: `heroku create some-unique-name`
4. `heroku stack:set container -a some-unique-name`
5. Attach MySQL: `heroku addons:create jawsdb:PLAN -a some-unique-name`
6. Set config, e.g. `ASPNETCORE_ENVIRONMENT=Production`, `Jwt__Secret=<long random string>`
7. `heroku git:remote -a some-unique-name` (or add remote manually)
8. `git push heroku main`

This repo includes **`Dockerfile`** and **`heroku.yml`** so Heroku knows how to build and run the .NET app. The app reads **`JAWSDB_URL`** automatically (see `HerokuConfig.cs`).

Optional helper (after `heroku login`): from repo root run `scripts/heroku-deploy.ps1`—it sets container stack, generates a JWT secret, sets config, and `git push heroku main` for **one** hard-coded app name (edit the script if you use a different app).

---

## If you typed the wrong password for `git.heroku.com`

That prompt is **Heroku**, not GitHub.

1. Windows: **Credential Manager** → **Windows Credentials** → remove any entry for `git.heroku.com`
2. In a terminal: `heroku login`
3. Retry: `git push heroku main`

Do **not** use your GitHub password for `git.heroku.com`.

---

## Summary for your goal

- **“On GitHub so people can open the project and run it”** → yes: they clone, configure local MySQL + `appsettings.Development.json`, `dotnet run`. Document that (this file + README Getting Started).
- **“Work with Heroku on their device”** → they use the **CLI** to **deploy** to **their** (or your team’s) Heroku app; the live site still runs **on Heroku**, not on their PC. Each deployer needs Heroku auth and usually their own app unless you share access.
