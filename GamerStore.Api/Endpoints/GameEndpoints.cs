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
    group.MapGet("/", async (GamerStoreContext dbContext) =>
      await dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto())
        .AsNoTracking()
        .ToListAsync());

    //GET /games/1
    group.MapGet("/{id}", async (int id, GamerStoreContext dbContext) =>
    {
      var game = await dbContext.Games.FindAsync(id);

      return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetTailsDto());
    })
    .WithName(GetGameEndpointName);

    //POST /games
    group.MapPost("/", async (CreateGameDto newGame, GamerStoreContext dbContext) =>
    {
      var game = newGame.ToEntity();
      game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);

      dbContext.Games.Add(game);
      await dbContext.SaveChangesAsync();

      return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameSummaryDto());
    });

    //Update /games/1
    group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GamerStoreContext dbContext) =>
    {
      var existingGame = await dbContext.Games.FindAsync(id);

      if (existingGame is null)
        return Results.NotFound();

      dbContext.Entry(existingGame)
               .CurrentValues
               .SetValues(updatedGame.ToEntity(id));

      await dbContext.SaveChangesAsync();

      return Results.NoContent();
    });

    //Delete /games/1
    group.MapDelete("/{id}", async (int id, GamerStoreContext dbContext) =>
    {
      await dbContext.Games
               .Where(game => game.Id == id)
               .ExecuteDeleteAsync();

      return Results.NoContent();
    });

    return group;
  }
}
