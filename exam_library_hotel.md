ЭКЗАМЕНАЦИОННОЕ ЗАДАНИЕ
ASP.NET Core Web API
Система управления библиотекой (Library Management System)

📘 Описание проекта
Создайте систему управления библиотекой с использованием ASP.NET Core Web API
и чистой архитектуры (Clean Architecture).

🧱 Таблицы базы данных

1. Books (Книги)

| Поле          | Тип данных | Описание                  |
|---------------|------------|---------------------------|
| Id            | int        | Первичный ключ            |
| Title         | string     | Название книги            |
| Author        | string     | Автор                     |
| Price         | decimal    | Цена                      |
| PublishedYear | int        | Год публикации            |
| CategoryId    | int        | Внешний ключ к Categories |

2. Categories (Категории)

| Поле        | Тип данных | Описание           |
|-------------|------------|--------------------|
| Id          | int        | Первичный ключ     |
| Name        | string     | Название категории |
| Description | string?    | Описание           |

3. Members (Члены библиотеки)

| Поле         | Тип данных | Описание          |
|--------------|------------|-------------------|
| Id           | int        | Первичный ключ    |
| FullName     | string     | Полное имя        |
| Email        | string     | Email адрес       |
| RegisteredAt | DateTime   | Дата регистрации  |

4. Borrows (Выдачи книг)

| Поле       | Тип данных | Описание                              |
|------------|------------|---------------------------------------|
| Id         | int        | Первичный ключ                        |
| BookId     | int        | Внешний ключ к Books                  |
| MemberId   | int        | Внешний ключ к Members                |
| BorrowDate | DateTime   | Дата выдачи                           |
| ReturnDate | DateTime?  | Дата возврата (null = не возвращена)  |

📌 Требования к архитектуре

Обязательно использовать следующие подходы:
- Clean Architecture (Domain, Application, Infrastructure, API)
- Repository Pattern — интерфейс + реализация для каждой таблицы
- Result Pattern — все методы сервисов возвращают Result<T>
- Pagination и Filtering — реализовать для GET /api/books
- Middleware — логирование запросов (метод, путь, время выполнения)
- Data Annotations — валидации на DTO (Required, MaxLength, Range и т.д.)
- Fluent API — настройка связей между таблицами через OnModelCreating
- LINQ — все запросы в сервисах только через LINQ
- Migrations — использовать EF Core Migrations

⚙️ CRUD эндпоинты

Реализовать полный CRUD для всех 4 таблиц:

| Метод  | Endpoint          | Описание                             |
|--------|-------------------|--------------------------------------|
| POST   | /api/[table]      | Создать запись                       |
| GET    | /api/[table]      | Список (с пагинацией для books)      |
| GET    | /api/[table]/{id} | Получить по Id                       |
| PUT    | /api/[table]/{id} | Обновить                             |
| DELETE | /api/[table]/{id} | Удалить                              |

🔟 Дополнительные API запросы

1. GET /api/books/filter
Фильтрация и пагинация книг по параметрам:
- searchTerm — поиск по названию или автору
- categoryId — фильтр по категории
- minPrice / maxPrice — диапазон цены
- page / pageSize — пагинация

2. GET /api/books/statistics
Общая статистика по книгам. Пример ответа:
```json
{ "totalBooks": 120, "averagePrice": 35.5, "totalBorrows": 340 }
```

3. GET /api/categories/with-books
Список категорий с вложенными книгами. Пример ответа:
```json
[ { "categoryId": 1, "categoryName": "Наука", "books": [...] } ]
```

4. GET /api/borrows/active
Список всех активных выдач (ReturnDate == null) с информацией о книге и члене.

5. GET /api/borrows/history?memberId=1
История выдач конкретного члена библиотеки.

6. GET /api/books/top-borrowed
Топ-5 самых часто выдаваемых книг. Пример ответа:
```json
[ { "bookTitle": "1984", "totalBorrows": 45 }, ... ]
```

7. GET /api/reports/monthly-borrows
Количество выдач по месяцам за последние 6 месяцев. Пример ответа:
```json
[ { "month": "2025-03", "count": 28 }, ... ]
```

8. GET /api/books/details/{id}
Полная информация о книге: категория, история выдач. Пример ответа:
```json
{ "id": 1, "title": "...", "category": "...", "borrows": [...] }
```

9. GET /api/members/top-readers
Топ-3 читателя по количеству взятых книг.

10. GET /api/dashboard/statistics
Общая статистика для дашборда. Пример ответа:
```json
{ "totalBooks": 120, "totalMembers": 45, "activeBorrows": 12, "totalRevenue": 4200 }
```

📁 Структура проекта

| Слой           | Проект         | Содержимое                                          |
|----------------|----------------|-----------------------------------------------------|
| Domain         | Domain/        | Entities, Enums                                     |
| Application    | Application/   | DTOs, Interfaces, Services, Results, Pagination     |
| Infrastructure | Infrastructure/| DbContext, Repositories, Migrations                 |
| API            | API/           | Controllers, Middleware, Program.cs                 |
