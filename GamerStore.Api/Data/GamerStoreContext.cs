using GamerStore.Api;
using GamerStore.Entities.Api;
using Microsoft.EntityFrameworkCore;

namespace GamerStore.Data.Api;

public class GamerStoreContext(DbContextOptions<GamerStoreContext> options) : DbContext(options)
{
  public DbSet<Game> Games => Set<Game>();
  public DbSet<Genre> Genres => Set<Genre>();
}
