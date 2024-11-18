using System;
using DayStoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DayStoreApi.Data;

public class DayStoreContext(DbContextOptions<DayStoreContext> options): DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

}
