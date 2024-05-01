using GamerStore.Data.Api;
using Microsoft.EntityFrameworkCore;

namespace GamerStore.Api.Data;

public static class DataExtensions
{
  public static async Task MigrateDbAsync(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<GamerStoreContext>();
    await dbContext.Database.MigrateAsync();
  }
}
