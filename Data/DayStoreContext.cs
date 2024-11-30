using System;
using DayStoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DayStoreApi.Data;

public class DayStoreContext(DbContextOptions<DayStoreContext> options): DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
             new { Id = 1, Name = "Simulation"},
             new { Id = 2, Name = "Sports"},
             new { Id = 3, Name = "Racing"},
             new { Id = 4, Name = "Shooting"},
             new { Id = 5, Name = "Adventure"}
        );
    }
}
