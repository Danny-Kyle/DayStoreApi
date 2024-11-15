using DayStoreApi.Dtos;

namespace DayStoreApi.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpointName = "GetGame";

private static readonly List<GameDto> games = [
    new(
        1,
        "EA FC 24",
        "Sports",
        69.99M,
        new DateOnly(2023, 09, 11)
    ),
    new(
        2,
        "F1 2024",
        "Sports",
        49.99M,
        new DateOnly(2024, 10, 27)
    ),
    new(
        3,
        "Madden NFL 24",
        "Sports",
        54.99M,
        new DateOnly(2023, 08, 15)
    ),
    new(
        4,
        "The Elder Scrolls V: Skyrim",
        "Shooter",
        69.99M,
        new DateOnly(2011, 11, 09)
    ),
    new(
        5,
        "It takes two",
        "Platform",
        43.99M,
        new DateOnly(2021, 03, 11)
    )
];

public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

    var group = app.MapGroup("games");

    // GET /games
group.MapGet("/", () => games);

// GET /games/1
group.MapGet("/{id}", (int id) => { GameDto? game = games.Find(game => game.Id == id); return game is null ? Results.NotFound() : Results.Ok(game); }).WithName(GetGameEndpointName);

//POST /games
group.MapPost("", (CreateGameDto newGame) =>
{
    GameDto game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

//PUT /games
group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if(index == -1){
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

//Delete /games/1
group.MapDelete("/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

return group;
}

}
