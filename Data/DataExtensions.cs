using System;
using Microsoft.EntityFrameworkCore;

namespace DayStoreApi.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app){
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DayStoreContext>();
        dbContext.Database.Migrate();
    }

}
