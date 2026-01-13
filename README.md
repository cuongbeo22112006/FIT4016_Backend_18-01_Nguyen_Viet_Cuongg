```markdown
# FIT4016-KiemTra-2026 / SchoolManagement (ASP.NET Core MVC)

This project is a web application (ASP.NET Core MVC + EF Core + SQLite) implementing the assignment requirements.

Features:
- Database: SQLite `SchoolManagement.db`.
- Tables: `schools` and `students` (1-to-many relationship).
- Seed data: at least 10 schools and 20 students (created automatically on first run).
- Students CRUD (Create, Read with pagination, Update, Delete).
- Server-side validation and user-friendly English error messages.
- Clean code and comments.

How to run:
1. Install .NET SDK (recommended .NET 7).
2. From the `SchoolManagement` folder:
   - `dotnet restore`
   - `dotnet build`
   - `dotnet run`
3. Open the app in browser: the console will show the listening URL (e.g. https://localhost:5001).
4. Use the "Students" page to manage student records.

Notes:
- Pagination: 10 students per page.
- Validation rules:
  - Full name: required, 2-100 chars.
  - Student ID: required, 5-20 chars, unique.
  - Email: required, valid, unique.
  - Phone: optional, if provided 10-11 digits.
  - School: required, must exist.
- If you want EF Migrations instead of EnsureCreated, run `dotnet ef migrations add Initial` and `dotnet ef database update` (install dotnet-ef tool if needed).

Git hints:
- Make at least 5 meaningful commits in English (required for submission).

If you want a zipped package or a GitHub repo with this code prepared, tell me and I will create it for you.
```