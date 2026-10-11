# PestanaDevApi

🌐 **English** | [Português](README.pt-BR.md)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![MariaDB](https://img.shields.io/badge/MariaDB-MySQL-003545?logo=mariadb&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-real--time-0078D4)
![Tests](https://img.shields.io/badge/tests-xUnit-5E2750)

REST API behind my portfolio website and other projects I build. It handles authentication (email/password and Google, GitHub and LinkedIn), user accounts, email confirmation, contact emails, a real-time **Scrum board**, and integrations with the **NASA** and **Metropolitan Museum of Art** public APIs.

---

## Table of contents

- [Features](#features)
- [Tech stack](#tech-stack)
- [Architecture](#architecture)
- [Getting started](#getting-started)
- [Configuration](#configuration)
- [API reference](#api-reference)
- [Real-time board (SignalR)](#real-time-board-signalr)
- [Email translations (i18n)](#email-translations-i18n)
- [Scheduled jobs](#scheduled-jobs)
- [Error handling](#error-handling)
- [Tests](#tests)
- [Author](#author)

---

## Features

- **Authentication**
  - Sign up and login with email and password (BCrypt hashes)
  - Login / sign up with **Google**, **GitHub** and **LinkedIn**
  - **JWT** access tokens + **refresh tokens** per device
  - **Google reCAPTCHA v3** on public forms
- **Email confirmation**: 4-character codes, stored as HMAC-SHA256 hashes, with expiration and resend
- **Contact form**: sends an email to the site owner and a confirmation email to the visitor, in **English or Portuguese** (JSON translations, next-i18next style)
- **User account**: view, update data, change password and delete account
- **Scrum board**
  - Boards with 5 default columns, tasks and members
  - Create tasks (with an optional deadline), move them between columns and reorder columns
  - Owner/member permissions
  - **Real-time updates** for every member through **SignalR**
  - Concurrency control (semaphore) and in-memory cache
- **NASA**: Astronomy Picture of the Day (single day or period)
- **Metropolitan Museum of Art**: artwork search
- **Infrastructure**: in-memory cache, rate limiting (500 requests / 30 min per IP), scheduled jobs (Quartz) and global exception handling

## Tech stack

| Area | Technology |
|---|---|
| Runtime / framework | .NET 8, ASP.NET Core Web API |
| Database | MariaDB / MySQL |
| Data access | Dapper + MySqlConnector (raw SQL, no ORM) |
| Real time | ASP.NET Core SignalR |
| Authentication | JWT Bearer, Google.Apis.Auth, GitHub OAuth, LinkedIn OpenID Connect |
| Security | BCrypt.Net, HMAC-SHA256, Google reCAPTCHA v3, rate limiting |
| Scheduled jobs | Quartz.NET (persistent store on MariaDB) |
| Emails | System.Net.Mail (SMTP) + HTML templates + JSON translations |
| Documentation | Swagger (Swashbuckle) |
| Tests | xUnit + Moq |

## Architecture

The code is split into layers. Each layer only talks to the one below it, through interfaces registered in the DI container.

```
Controllers  →  Services  →  Repositories  →  MariaDB
   (HTTP)       (business      (Dapper +
                  rules)        raw SQL)
```

```
PestanaDevApi/
├── AppConfig/          # Database, Quartz and secrets setup
├── Constants/          # Messages, SQL queries (partial classes), cache keys, etc.
├── Controllers/        # HTTP endpoints
├── Dtos/               # Request, response and notification DTOs
├── EmailTemplates/     # Email HTML templates
├── Exceptions/         # ApiException
├── Extensions/         # Extension methods (DTO → SQL params, validations)
├── Hubs/               # SignalR hubs (BoardHub)
├── Interfaces/         # Service, repository and hub contracts
├── Locales/            # Translations (en/email.json, pt/email.json)
├── Models/             # Domain models and enums
├── QuartzJobs/         # Scheduled jobs
├── Repositories/       # Data access (Dapper)
├── Services/           # Business rules, cache, notifications, integrations
└── Utils/              # Helpers (hash, dates, HTTP client, etc.)

PestanaDevApi.Tests/    # Unit tests (xUnit + Moq)
```

Conventions:

- **Responses**: every service returns a DTO that inherits from `DefaultResponseDto` (`IsSuccess`, `StatusCode`, `Message`); controllers turn failures into HTTP results with `HandleFailure()`.
- **SQL**: queries live in `Constants/Queries` as constants. `BoardQueries` is a partial class split into `Selects`, `Inserts`, `Updates` and `Deletes`.
- **Error messages**: each one has a unique code (`"6A3C : Board was not found!"`), which makes them easy to trace.

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MariaDB (or MySQL) with two databases: one for the application and one for Quartz
- Credentials for the external services you plan to use (SMTP, Google, GitHub, LinkedIn, reCAPTCHA, NASA)

### Database

Main application tables:

| Table | Purpose |
|---|---|
| `USERS_PROFILE_DATA` | Users |
| `USERS_PROFILE_PLATFORM_DATA` | Link between a user and Google/GitHub/LinkedIn |
| `CONFIRMATION_CODE` | Email confirmation codes (hashed) |
| `CONFIRMED_EMAILS` | Confirmed emails |
| `REFRESH_TOKEN` | Refresh tokens per device |
| `BOARD`, `BOARD_USERS`, `BOARD_COLUMNS`, `BOARD_TASKS` | Scrum board |

> 🔒 **The database creation script is private** and isn't in this repository. To get it, request it from the project owner ([Vitor Pestana](https://github.com/vitorpestana94)).

Quartz tables are created with the [official Quartz.NET script for MySQL](https://github.com/quartznet/quartznet/blob/main/database/tables/tables_mysql_innodb.sql).

### Running

```bash
git clone https://github.com/vitorpestana94/PestanaDevApi.git
cd PestanaDevApi

# create the configuration files (see "Configuration")

dotnet run --project PestanaDevApi --launch-profile http
```

The API starts at `http://localhost:5024`. In the `Development` environment, Swagger is available at `http://localhost:5024/swagger`.

## Configuration

Settings are read from `.properties` files (`key=value`), which are **not versioned** (they're in `.gitignore`):

```
PestanaDevApi/config/
├── app/
│   ├── config.properties                # production
│   └── config.Development.properties    # development
└── app-secret/
    ├── config.properties
    └── config.Development.properties
```

The file is picked by the environment: `config.{Environment}.properties` for `Development` and `Local`, `config.properties` otherwise.

**`config/app`**

```properties
projectname=PestanaDevApi
api.baseUrl=https://your-api-url
database.url=localhost
database.username=your-user
database.databasename=your-database
database.quartz.databasename=your-quartz-database
google.clientid.web=your-google-client-id
github.appname=your-github-app-name
github.endpoint=https://api.github.com
linkedin.publickeysurl=https://www.linkedin.com/oauth/openid/jwks
linkedin.validissuser=https://www.linkedin.com/oauth
linkedin.clientid=your-linkedin-client-id
email.address=your-email@domain.com
email.apppassword=your-smtp-app-password
email.smtp=smtp.your-provider.com
nasa.endpoint=https://your-nasa-apod-endpoint
metropolitanMuseum.endpoint=https://collectionapi.metmuseum.org/public/collection
```

**`config/app-secret`**

```properties
database.password=your-database-password
jwt.key=base64-key-of-at-least-32-bytes
jwt.issuer=your-issuer
sha.secret=secret-for-confirmation-codes-hmac
captchaSecret=your-recaptcha-v3-secret
```

> ⚠️ Never commit these files. `jwt.key` must be Base64.

## API reference

🔒 = requires the `Authorization: Bearer <jwt>` header.

### Auth — `/auth`

| Method | Route | Description | |
|---|---|---|---|
| POST | `/auth` | Login with email and password (reCAPTCHA) | |
| POST | `/auth/oauth` | Login / sign up with Google, GitHub or LinkedIn | |
| POST | `/auth/signup` | Sign up (requires a confirmed email) | |
| GET | `/auth/isEmailRegistered/{email}` | Check whether an email is already registered | |
| POST | `/auth/forgot-password` | Reset the password (requires a confirmed email) | |
| POST | `/auth/refresh-token` | Get a new token from a refresh token | |
| DELETE | `/auth` | Logout (deletes the device's refresh token) | 🔒 |

### Email and confirmation — `/email`, `/confirmation`

| Method | Route | Description |
|---|---|---|
| POST | `/email/contact` | Send the contact email (owner + visitor) |
| POST | `/email/confirmation` | Send a confirmation code |
| POST | `/email/confirmation/resend` | Resend the confirmation code |
| POST | `/confirmation` | Validate the received code |
| GET | `/confirmation/sent/{email}` | Check whether a code was already sent |

### User — `/user` 🔒

| Method | Route | Description |
|---|---|---|
| GET | `/user` | Logged user's data |
| PATCH | `/user` | Update name and/or email |
| PATCH | `/user/password` | Change password |
| DELETE | `/user` | Delete account |

### Scrum board — `/board` 🔒

| Method | Route | Description | Who can |
|---|---|---|---|
| POST | `/board` | Create a board (5 columns + first task) | Any user |
| GET | `/board` | List the user's boards | Owner |
| GET | `/board/{id}` | Board data (columns, tasks, members) | — |
| PATCH | `/board/{boardId}` | Update name/description | Owner |
| DELETE | `/board/{boardId}` | Delete the board | Owner |
| POST | `/board/{boardId}/tasks` | Create a task on a column (returns the created task) | Member |
| PATCH | `/board/{boardId}/tasks/{taskId}` | Update or move a task | Member |
| DELETE | `/board/{boardId}/tasks/{taskId}` | Delete a task | Member |
| PATCH | `/board/{boardId}/columns/{columnId}` | Rename or reorder a column | Member |
| GET | `/board/{boardId}/members` | List members | Member |
| POST | `/board/{boardId}/members` | Add a member by email | Owner |
| DELETE | `/board/{boardId}/members/{userId}` | Remove a member (or leave the board) | Owner / the member |

### Integrations

| Method | Route | Description | |
|---|---|---|---|
| GET | `/nasa?date=yyyy-MM-dd` | Astronomy Picture of the Day (today if no date) | 🔒 |
| GET | `/nasa/period?startDate=...&endDate=...` | Astronomy pictures of a period | 🔒 |
| GET | `/metropolitan-museum?search=...` | Artwork search | |

## Real-time board (SignalR)

Changes to the board are still made through the REST endpoints. SignalR only **pushes** those changes to the other members.

- **Hub:** `/hubs/board` (requires the JWT, sent as `access_token` in the query string)
- **Methods the client calls:** `JoinBoard(boardId)` (members only) and `LeaveBoard(boardId)`
- **Payload of every event:** `{ boardId, changedBy, data }`

| Event | `data` | Sent after |
|---|---|---|
| `BoardUpdated` | board | `PATCH /board/{id}` |
| `BoardDeleted` | `boardId` | `DELETE /board/{id}` |
| `TaskCreated` | created task | `POST .../tasks` |
| `TaskUpdated` | updated task | `PATCH .../tasks/{taskId}` |
| `TaskDeadlineChanged` | `{ taskId, taskName, oldDeadline, newDeadline }` | `PATCH .../tasks/{taskId}` that changed the deadline |
| `TaskDeleted` | `taskId` | `DELETE .../tasks/{taskId}` |
| `ColumnsUpdated` | all the columns | `PATCH .../columns/{columnId}` |
| `MemberAdded` | `{ id, name, email, isOwner }` | `POST .../members` |
| `MemberRemoved` | `memberId` | `DELETE .../members/{userId}` |

TypeScript client example:

```ts
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${API_URL}/hubs/board`, { accessTokenFactory: () => getAccessToken() })
  .withAutomaticReconnect()
  .build();

connection.on("TaskUpdated", (n) => {
  if (n.changedBy !== myUserId) upsertTask(n.data); // ignore your own changes
});

await connection.start();
await connection.invoke("JoinBoard", boardId);
connection.onreconnected(() => connection.invoke("JoinBoard", boardId));
```

> Connections, groups, the semaphore and the cache live in memory, so the current setup supports **a single API instance**. Running more instances would require a backplane (e.g. Redis).

## Email translations (i18n)

Email texts live in JSON files, following the **next-i18next** conventions:

```
Locales/
├── en/email.json
└── pt/email.json
```

```csharp
_translationService.Translate("pt", "email:contact.client.title", new() { ["name"] = "Ana" }); // "Olá, Ana!"
```

- Keys in the `namespace:path.to.key` format
- `{{variable}}` interpolation
- Falls back to English when the locale or the key doesn't exist

To add a language, create `Locales/{language}/email.json` with the same keys.

## Scheduled jobs

| Job | Schedule (UTC) | Purpose |
|---|---|---|
| `DeleteUnfreshConfirmationCodesJob` | Every 15 minutes | Delete expired confirmation codes |
| `DeleteExpiredRefreshTokensJob` | Daily at 3:00 | Delete expired refresh tokens |
| `NasaCacheRefreshJob` | Daily at 4:00 | Refresh the NASA cache |
| `MetropolitanMuseumCacheRefreshJob` | Daily at 4:30 | Refresh the Metropolitan Museum cache |

## Error handling

Errors come back in the following format:

```json
{ "message": "6A3C : Board was not found!" }
```

| Situation | Status |
|---|---|
| Invalid request / business rule | 400 |
| Not authenticated / invalid credentials | 401 |
| No permission / reCAPTCHA failed | 403 |
| Resource not found | 404 |
| Unexpected error | 500 |

Model validation errors return `{ "status": 400, "message": "Validation error" }`.

## Tests

The `PestanaDevApi.Tests` project has unit tests (xUnit + Moq) for services, extensions and utils. They need no database or network.

```bash
dotnet test
```

## Author

**Vitor Pestana**, [GitHub](https://github.com/vitorpestana94)
