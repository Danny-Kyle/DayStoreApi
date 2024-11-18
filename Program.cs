using DayStoreApi.Data;
using DayStoreApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=DayStore.db";
builder.Services.AddSqlite<DayStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
