using DayStoreApi.Data;
using DayStoreApi.Dtos;
using DayStoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DayStoreApi.Endpoints;

public static class GamesEndpoint
{
    private const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", async (DayStoreContext db, CancellationToken ct) =>
            await db.Games.AsNoTracking().OrderBy(game => game.Id)
                .Select(game => new GameDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.RealeaseDate))
                .ToListAsync(ct));

        group.MapGet("/{id:int}", async (int id, DayStoreContext db, CancellationToken ct) =>
        {
            var game = await db.Games.AsNoTracking().Include(game => game.Genre)
                .SingleOrDefaultAsync(game => game.Id == id, ct);
            return game is null ? Results.NotFound() : Results.Ok(ToDto(game));
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto input, DayStoreContext db, CancellationToken ct) =>
        {
            var genre = await FindGenre(db, input.Genre, ct);
            if (genre is null) return InvalidGenre();

            var game = new Game
            {
                Name = input.Name,
                Genre = genre,
                GenreId = genre.Id,
                Price = input.Price,
                RealeaseDate = input.ReleaseDate
            };
            db.Games.Add(game);
            await db.SaveChangesAsync(ct);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, ToDto(game));
        });

        group.MapPut("/{id:int}", async (int id, UpdateGameDto input, DayStoreContext db, CancellationToken ct) =>
        {
            var game = await db.Games.FindAsync([id], ct);
            if (game is null) return Results.NotFound();
            var genre = await FindGenre(db, input.Genre, ct);
            if (genre is null) return InvalidGenre();

            game.Name = input.Name;
            game.GenreId = genre.Id;
            game.Price = input.Price;
            game.RealeaseDate = input.ReleaseDate;
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, DayStoreContext db, CancellationToken ct) =>
        {
            await db.Games.Where(game => game.Id == id).ExecuteDeleteAsync(ct);
            return Results.NoContent();
        });

        return group;
    }

    private static Task<Genre?> FindGenre(DayStoreContext db, string name, CancellationToken ct) =>
        db.Genres.FirstOrDefaultAsync(genre => EF.Functions.Collate(genre.Name, "NOCASE") == name.Trim(), ct);

    private static IResult InvalidGenre() => Results.ValidationProblem(
        new Dictionary<string, string[]> { ["genre"] = ["Choose a genre from the seeded catalog."] });

    private static GameDto ToDto(Game game) =>
        new(game.Id, game.Name, game.Genre!.Name, game.Price, game.RealeaseDate);
}
