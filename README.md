# ✅ Task Management Application (Angular + ASP.NET Core)

A full-stack Task Management web application built with **Angular** (Signals, Standalone Components) and **ASP.NET Core Web API**, featuring role-based access control (Admin & User), task filtering, priority tracking, and full CRUD operations.

---

## 🚀 Features

- **Role-Based Access Control (Admin & User)**:
  - **Admin**: View all tasks across users, search tasks by username, create, edit, and delete tasks.
  - **User**: View personal tasks, add new tasks, and delete tasks.
- **Task Tracking & Status Badges**:
  - Priority levels (`Low`, `Medium`, `High`) with color-coded badges.
  - Status tracking (`Pending`, `In Progress`, `Completed`) with creation and due dates.
- **Modern Dark-Slate UI**:
  - Responsive glassmorphic navigation bar, interactive search bar, and clean dark-themed forms.
- **Reactive State Management**:
  - Built using Angular Signals (`signal`, `computed`) for instant UI updates without unnecessary page reloads.

---

## 🛠️ Tech Stack

- **Frontend**: Angular (v21+), TypeScript, HTML5, CSS3, RxJS
- **Backend**: ASP.NET Core Web API (.NET), C#, Entity Framework Core
- **Database**: SQL Server

---

## 📁 Project Structure

```text
Task-Management-App/
├── Backend/        # ASP.NET Core Web API (Controllers, Services, DTOs, Models)
└── Frontend/       # Angular SPA (Components, Services, Routing)
```

---

## ⚙️ Getting Started (Local Setup)

### Prerequisites
- [Node.js](https://nodejs.org) (v20 or higher) & Angular CLI (`npm install -g @angular/cli`)
- [.NET SDK](https://microsoft.com) (v8.0 or higher)
- SQL Server / SQL Server Express

### 1. Backend Setup (.NET Core API)
1. Navigate to the backend directory:
   ```bash
   cd Backend
   ```
2. Update the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TaskAppDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
3. Apply Entity Framework migrations (if applicable) and run the API:
   ```bash
   dotnet ef database update
   dotnet run
   ```
4. Note the API base URL (e.g., `https://localhost:7001/api`).

### 2. Frontend Setup (Angular)
1. Navigate to the frontend directory:
   ```bash
   cd Frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Verify the `apiURL` inside `src/app/Services/task-services.ts` matches your running .NET backend port.
4. Start the Angular development server:
   ```bash
   ng serve
   ```
5. Open your browser at `http://localhost:4200`.

---

## 📸 Screenshots

*(Add screenshots of your Login page, Task List dashboard, and Edit Task form here)*

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).
