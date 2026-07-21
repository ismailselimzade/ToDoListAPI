# To-Do List API

A REST API for managing personal to-do items with user authentication.  
İstifadəçi autentifikasiyası ilə şəxsi tapşırıqları idarə etmək üçün REST API.

---

## Features / Xüsusiyyətlər

- User authentication with JWT — only authenticated users can access their own to-do items.  
  JWT ilə istifadəçi autentifikasiyası — yalnız autentifikasiya olunmuş istifadəçilər öz tapşırıqlarına daxil ola bilər.

- Full CRUD support for to-do items.  
  Tapşırıqlar üçün tam CRUD dəstəyi.

- Filter to-do items by status (Pending, In Progress, Completed).  
  Tapşırıqları statusa görə filtrləmək (Gözləyir, Davam edir, Tamamlandı).

- Each user can only see and modify their own to-do items.  
  Hər istifadəçi yalnız öz tapşırıqlarını görüb dəyişə bilər.

---

## What I Learned / Öyrəndiklərim

- How JWT authentication works and how to implement it in ASP.NET Core.  
  JWT autentifikasiyasının necə işlədiyini və ASP.NET Core-da necə tətbiq ediləcəyini öyrəndim.

- How to hash passwords and verify them securely.  
  Şifrələri necə hash etmək və təhlükəsiz şəkildə yoxlamaq olar.

- How to use the [Authorize] attribute to protect endpoints.  
  Endpoint-ləri qorumaq üçün [Authorize] atributunun necə istifadə olunacağını öyrəndim.

- How to get the current user's ID from the JWT token inside a controller.  
  Controller-in içindən JWT token-dən cari istifadəçinin ID-sini necə almaq olar.

- How to use DTOs to control what data is sent and received.  
  Göndərilən və alınan məlumatları idarə etmək üçün DTO-ların necə istifadə olunacağını öyrəndim.

---

## Tech Stack / Texnologiyalar

C#, ASP.NET Core, Entity Framework Core, SQL Server, JWT

---

## Endpoints

### Auth

| Method | URL | Description |
|--------|-----|-------------|
| POST | `/api/auth/login` | Returns a JWT token. / JWT token qaytarır. |

### To-Do Items

| Method | URL | Description |
|--------|-----|-------------|
| POST | `/api/todo` | Creates a new to-do item. / Yeni tapşırıq yaradır. |
| GET | `/api/todo/user` | Returns all to-do items of the current user. / Cari istifadəçinin bütün tapşırıqlarını qaytarır. |
| GET | `/api/todo/{id}` | Returns a specific to-do item. / Müəyyən tapşırığı qaytarır. |
| PUT | `/api/todo/{id}` | Updates a specific to-do item. / Müəyyən tapşırığı yeniləyir. |
| DELETE | `/api/todo/{id}` | Deletes a specific to-do item. / Müəyyən tapşırığı silir. |
| GET | `/api/todo/status/{status}` | Returns to-do items filtered by status. / Statusa görə tapşırıqları qaytarır. |

---

## Setup / Quraşdırma

1. Clone the repository.  
   Repozitorini kopyalayın.

2. Add the connection string and JWT settings to `appsettings.json`.  
   `appsettings.json` faylına connection string və JWT parametrlərini əlavə edin.

3. Apply the database migrations.  
   Veritabanı miqrasiyalarını tətbiq edin.

    `dotnet ef database update`

4. Run the project.  
   Proyekti işə salın.

   `dotnet run`
