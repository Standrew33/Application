# ASP.NET Core Web API + Razor Pages (JWT Authentication)

This repository contains a simple demonstration of authentication using **JWT Bearer tokens** with **ASP.NET Core Web API** and **ASP.NET Core Razor Pages**.

- The **Web API** is responsible for authentication and protected data
- The **Razor Pages app** acts as the user interface
- Razor Pages communicates with the API **exclusively via HTTP**
- The JWT access token is securely stored inside an **HttpOnly authentication cookie**

The project is intended as a **simple educational example** and does not use database or advanced authentication scenarios.

---

## How to run the project

### Requirements
- .NET SDK **8.0** or newer
- Git

### Steps

1. Clone the repository:
```bash
git clone https://github.com/Standrew33/Application.git
2. Build the solution:
```bash
dotnet build
3. Run the **Api** project (Web API):
```bash
dotnet run --project Api
After startup, note the HTTP port (e.g. http://localhost:5277)
4. Verify API base URL in:
```bash
Client/appsettings.json
```bash
"Api": {
  "BaseUrl": "http://localhost:5277"
}
5. Run the **Client** project (Razor Pages):
```bash
dotnet run --project Client
6. Open your browser and navigate to:
```bash
http://localhost:5085
