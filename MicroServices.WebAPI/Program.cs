using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shift_System.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("gatewayocelot.json", false, false);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddJwtAuthentication();

var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
