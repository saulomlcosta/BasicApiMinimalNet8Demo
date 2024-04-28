using GamerStore.Api.Dtos;
using GamerStore.Entities.Api;

namespace GamerStore.Api.Mappings;

public static class GameMapping
{
  public static Game ToEntity(this CreateGameDto gameDto)
  {
    return new Game()
    {
      Name = gameDto.Name,
      GenreId = gameDto.GenreId,
      Price = gameDto.Price,
      ReleaseDate = gameDto.ReleaseDate
    };
  }

  public static Game ToEntity(this UpdateGameDto gameDto, int id)
  {
    return new Game()
    {
      Id = id,
      Name = gameDto.Name,
      GenreId = gameDto.GenreId,
      Price = gameDto.Price,
      ReleaseDate = gameDto.ReleaseDate
    };
  }

  public static GameDetailsDto ToGameDetTailsDto(this Game game)
  {
    return new GameDetailsDto(
      game.Id,
      game.Name,
      game.GenreId,
      game.Price,
      game.ReleaseDate
    );
  }

  public static GameSummaryDto ToGameSummaryDto(this Game game)
  {
    return new GameSummaryDto(
      game.Id,
      game.Name,
      game.Genre!.Name,
      game.Price,
      game.ReleaseDate
    );
  }
}
