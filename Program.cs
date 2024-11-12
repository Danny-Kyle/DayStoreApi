using DayStoreApi.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int Id) => games.Find(game => game.Id == Id)).WithName(GetGameEndpointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame)=> {
    GameDto game = new (games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

app.Run();
