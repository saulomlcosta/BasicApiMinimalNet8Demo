using GamerStore.Api.Mappings;
using GamerStore.Data.Api;
using GamerStore.Entities.Api;
using Microsoft.EntityFrameworkCore;

namespace GamerStore.Api.Endpoints;

public static class GenreEndpoints
{
  public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("genres");

    group.MapGet("/", async (GamerStoreContext dbContext) =>
      await dbContext.Genres
      .Select(genre => genre.ToDto())
      .AsNoTracking()
      .ToListAsync());

    return group;
  }
}
