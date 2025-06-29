# Mero Doctor Project

**MeroDoctorProject** is a web-based medical platform built with **ASP.NET Core Web API** that enables secure interaction between doctors and patients. It provides features like appointment booking, doctor availability tracking, X-ray analysis, blogging, feedback collection, and role-based access control.

Additionally, it integrates with a Python Flask-based machine learning model to detect pneumonia from X-ray images and recommends the top 5 nearest hospitals based on the user’s location.

---

## 🔑 Key Features

- JWT-based authentication for Doctors and Patients
- Doctor registration, specialization & weekly availability setup
- Book, view, and manage appointments
- Upload and analyze X-ray images for pneumonia detection (Flask ML service)
- Automatic hospital recommendation based on user location (top 5 nearby)
- Blog system with comments, likes, and ratings
- Feedback and doctor review system
- Role-based access for Admin, Doctor, and Patient
- eSewa payment integration before confirming appointments
- Admin validation of doctors using NMC (NOC) number,Manage (Doctor Specialization, BlogCategory),View total users

---

## 🏗️ Architecture

- **Backend:** ASP.NET Core Web API
- **Authentication:** ASP.NET Identity with JWT
- **Design Pattern:** Repository Pattern 
- **Image Handling:** Static file storage in `wwwroot/images`
- **Machine Learning:** Python Flask (Pneumonia Detection via X-ray)
- **Payment Integration:** eSewa for appointment validation
- **Communication:** Flask API called from ASP.NET Core
- **DTOs:** Used for data transfer between layers

---

## 🧰 Technologies Used

- **Language:** C#
- **Framework:** ASP.NET Core 8 (Web API)
- **Database:** SQL Server
- **ORM:** Entity Framework Core (Code First)
- **ML:** Python Flask + DenseNet + Grad-CAM
- **Authentication:** ASP.NET Identity + JWT
- **Other Tools:** LINQ, AutoMapper, SignalR
- **Payments:** eSewa Payment Gateway

---

## ⚙️ Setup Instructions

### 1. Update the Connection String

Open `appsettings.json` and replace the server name with your actual SQL Server instance:

```json
  "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME; Database=MeroDoctorAppDb; Trusted_Connection=True; TrustServerCertificate=True; Connection Timeout=30; Max Pool Size=200; MultipleActiveResultSets=True"
  }
```
### 2. Update the Database

Run the following command to apply migrations and create the database:

`dotnet ef database update`


### 3. Build and Run the Project

`dotnet build`
`dotnet run`
