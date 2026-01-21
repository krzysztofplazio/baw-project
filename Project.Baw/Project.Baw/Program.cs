using Project.Baw.Endpoints;
using Project.Baw.Extensions;
using Project.Baw.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddOpenIddictAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await MigrationHelpers.MigrateDatabase(services);
    await OpenIddictHelpers.SeedAsync(services);
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapOpenIddictEndpoints();
app.MapReservationEndpoints();

app.Run();
