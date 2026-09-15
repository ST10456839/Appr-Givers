# Gift of the Givers — Prototype Web Application (APPR6312 Part 1, Section 3.2)

An ASP.NET Core MVC (.NET 8) prototype for the Gift of the Givers disaster relief platform,
covering branding, authentication with roles, a donation flow with a placeholder tax
certificate, and a volunteer sign-up form with an employee dashboard.

## What's included (mapped to the brief)

| Brief requirement | Where it lives |
|---|---|
| Logo & header, nav bar, responsive Bootstrap layout | `Views/Shared/_Layout.cshtml` |
| ASP.NET Identity authentication | `Controllers/AccountController.cs`, `Views/Account/*` |
| Employee role (post updates, view volunteer sign-ups) | `Controllers/EmployeeController.cs`, `Views/Employee/*` |
| Donor role / anonymous guest donation | `Controllers/DonateController.cs`, `Views/Donate/*` |
| One-time / recurring donation, ZAR/USD/EUR currency | `Models/ViewModels/DonationViewModel.cs`, `Views/Donate/Index.cshtml` |
| Placeholder tax certificate (downloadable PDF) | `DonateController.Certificate()` (uses QuestPDF) |
| Volunteer registration form (name, skills, availability) | `Controllers/VolunteerController.cs`, `Views/Volunteer/*` |
| Minimal data persistence (simple SQL tables) | SQLite via EF Core, `Data/ApplicationDbContext.cs` |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- No external database server needed — the prototype uses a local SQLite file
  (`gotg.db`), created automatically the first time you run it.

## Running locally

```bash
cd GiftOfTheGivers
dotnet restore
dotnet run
```

Then open the URL shown in the console (e.g. `https://localhost:7223`).

On first run the app automatically:
- Creates `gotg.db` and all tables (no `dotnet ef` step required for this prototype),
- Seeds the **Employee** and **Donor** roles,
- Seeds a demo Employee account: **employee@gotg.org** / **Employee123!**
- Seeds two sample relief-project updates so the Home page isn't empty.

To try the Donor flow, either register a new **Donor** account at `/Account/Register`,
or just donate as a guest (tick "Donate anonymously" or leave your name blank — no
login is required to donate).

## Suggested demo script (for your screenshots)

1. **Home** — shows branding, nav bar, and latest project updates.
2. **Register** as a Donor, then **Login**.
3. **Donate** — pick an amount, currency, one-time/recurring, submit, then download
   the placeholder tax certificate PDF from the Thank You page.
4. **Volunteer** — submit the sign-up form as a guest.
5. **Logout**, then **Login** with the demo Employee account.
6. **Employee Dashboard** — shows the volunteer sign-up and donation you just created;
   click "Post a Project Update" to add a new update, which then appears on Home.

Take screenshots at each step per the "Evidence to submit" checklist in 3.2.

## Publishing to Azure App Services (Section 3.3)

1. In Visual Studio: right-click the project → **Publish** → **Azure** → **Azure App Service (Windows/Linux)** → create or pick a resource group and App Service plan.
2. Or via CLI:
   ```bash
   az login
   az webapp up --name <your-app-name> --resource-group <your-rg> --runtime "DOTNETCORE:8.0"
   ```
3. **Important:** SQLite's local file won't persist reliably across App Service restarts/scaling.
   For the deployed version, either:
   - Switch the connection string in `appsettings.json` to an **Azure SQL Database** (recommended,
     and ties in directly with Section B of Part 1), or
   - Accept that the free-tier demo will reset data on restart (fine for a prototype demo).
4. Take screenshots of the deployment process and record the live URL as required by 3.3.

## Notes for your group submission

- This prototype intentionally keeps persistence and payment processing simple/dummy,
  per the brief ("data persistence can be minimal... full logic will be developed in Part 2").
- Password rules are relaxed for ease of demoing — tighten these for Part 2 / production.
- Registration lets a user pick "Employee" or "Donor" for demo convenience; in a real
  system, Employee accounts would be provisioned by an administrator, not self-selected.
- Remember to add your IIE-style reference list and group contribution reflections
  (who worked on UI, auth, donation logic) as required by the brief.
