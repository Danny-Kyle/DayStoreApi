## DayStoreApi

- Install the .NET 8 SDK. Run `dotnet restore`, `dotnet build`, and `dotnet run`.
- The API uses EF Core with SQLite and applies migrations on startup. Set `ConnectionStrings__DayStore` to a writable SQLite path when you need a separate database.
- `/games` supports list, get, create, update, and delete. Use a seeded genre name such as Sports or Racing. The API matches names without regard to letter case.
- Verified against fresh SQLite: migrations, validation, CRUD, persistence across an API restart, and new IDs after deletion.
