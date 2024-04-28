using GamerStore.Api.Dtos;
using GamerStore.Api.Mappings;
using GamerStore.Data.Api;
using Microsoft.EntityFrameworkCore;

namespace GamerStore.Api;

public static class GameEnpoints
{
  const string GetGameEndpointName = "GetGame";

  public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("games")
                  .WithParameterValidation();


    //GET /games
    group.MapGet("/", (GamerStoreContext dbContext) => dbContext.Games
    .Include(game => game.Genre)
    .Select(game => game.ToGameSummaryDto())
    .AsNoTracking());

    //GET /games/1
    group.MapGet("/{id}", (int id, GamerStoreContext dbContext) =>
    {
      var game = dbContext.Games.Find(id);

      return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetTailsDto());
    })
    .WithName(GetGameEndpointName);

    //POST /games
    group.MapPost("/", (CreateGameDto newGame, GamerStoreContext dbContext) =>
    {
      var game = newGame.ToEntity();
      game.Genre = dbContext.Genres.Find(newGame.GenreId);

      dbContext.Games.Add(game);
      dbContext.SaveChanges();

      return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameSummaryDto());
    });

    //Update /games/1
    group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GamerStoreContext dbContext) =>
    {
      var existingGame = dbContext.Games.Find(id);

      if (existingGame is null)
        return Results.NotFound();

      dbContext.Entry(existingGame)
               .CurrentValues
               .SetValues(updatedGame.ToEntity(id));

      dbContext.SaveChanges();

      return Results.NoContent();
    });

    //Delete /games/1
    group.MapDelete("/{id}", (int id, GamerStoreContext dbContext) =>
    {
      dbContext.Games
               .Where(game => game.Id == id)
               .ExecuteDelete();

      return Results.NoContent();
    });

    return group;
  }
}
