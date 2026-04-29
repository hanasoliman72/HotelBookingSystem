# 🏨 Hotel Booking System

> A web-based hotel booking system built with ASP.NET Core MVC, featuring role-based access control and real-time booking validation.

---

## 📖 Overview

**Hotel Booking System** is a full-stack web application that allows users to browse available hotel rooms and make bookings online. Admins have a dedicated dashboard to manage room listings, while guests can register, log in, and reserve rooms with real-time availability checks powered by Entity Framework and SQL Server.

---

## ✨ Features

- 🛏️ **Room Browsing** — Users can view all available rooms with details and availability status
- 📅 **Room Booking** — Authenticated users can book rooms with real-time validation
- 🔐 **User Authentication** — Secure registration and login system
- 👥 **Role-Based Access Control** — Separate permissions for Admins and regular Users
- 🛠️ **Admin Dashboard** — Admins can add, edit, and delete room listings
- ✅ **Real-Time Booking Validation** — Prevents double-booking using Entity Framework queries

---

## 🛠️ Tech Stack

| Technology | Role |
|---|---|
| ASP.NET Core MVC | Web framework & routing |
| C# | Backend logic |
| Entity Framework Core | ORM & database operations |
| SQL Server | Database |
| HTML / CSS | Frontend views & styling |
| ASP.NET Identity | Authentication & authorization |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download) or later
- SQL Server (or SQL Server Express)
- Visual Studio 2022 / VS Code

### Setup

```bash
# Clone the repository
git clone https://github.com/hanasoliman72/hotel-booking-system.git
cd hotel-booking-system
```

**Configure the database connection** in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HotelBookingDB;Trusted_Connection=True;"
}
```

**Apply migrations and seed the database:**

```bash
dotnet ef database update
```

**Run the application:**

```bash
dotnet run
```

Then open `https://localhost:5001` in your browser.

---

## 👥 User Roles

| Role | Permissions |
|---|---|
| **Guest** | Browse rooms |
| **User** | Browse rooms, make & view bookings |
| **Admin** | Full access — manage rooms, view all bookings |
## 📄 License

This project is for educational purposes.
