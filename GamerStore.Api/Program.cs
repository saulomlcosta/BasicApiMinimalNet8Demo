using GamerStore.Api;
using GamerStore.Api.Data;
using GamerStore.Api.Endpoints;
using GamerStore.Data.Api;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GamerStore");
builder.Services.AddSqlite<GamerStoreContext>(connString);

var app = builder.Build();

app.MapGameEndpoints();
app.MapGenreEndpoints();

await app.MigrateDbAsync();

app.Run();
