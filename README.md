# Mero Doctor Project

**MeroDoctorProject** is a web-based medical platform built with ASP.NET Core Web API that enables secure interaction between doctors and patients. It provides features like appointment booking, doctor availability tracking, X-ray analysis management, a blogging system, feedback collection, and role-based access control.

---

## 🔑 Key Features

- JWT-based authentication for doctors and patients
- Doctor registration, specialization & weekly availability
- Book and manage appointments
- Upload and manage X-ray records (with image support)
- Medical blogs with comments, likes, and ratings
- Feedback submission system
- Role-based access for doctors, patients, and admins

---

## 🏗️ Architecture

- **Backend:** ASP.NET Core Web API (MVC pattern)
- **Design Pattern:** Repository Pattern with optional service layer
- **Image Handling:** ImageController + static file storage (wwwroot/images)
- **Authentication:** ASP.NET Identity + JWT Token
- **Data Transfer:** DTOs for clean API contracts
- **Database Access:** Entity Framework Core (Code First)

---

## 🧰 Technologies Used

- **Language:** C# 
- **Framework:** ASP.NET Core 8  (Web API)
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Auth:** JWT with ASP.NET Identity
- **Other:** LINQ, AutoMapper, Role-based Authorization,Singlar

---
## ⚙️ Setup Instructions

### 1. Update the Connection String

Open `appsettings.json` and replace the SQL Server name with yours:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MeroDoctorDb;Trusted_Connection=True;"
}

### 2. Update the Database

Run the following command to apply migrations and create the database:

`dotnet ef database update`


### 3. Build and Run the Project

`dotnet build`
`dotnet run`