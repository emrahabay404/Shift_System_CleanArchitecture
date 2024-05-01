using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shift_System.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("gatewayocelot.json", false, false);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddJwtAuthentication();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

await app.UseOcelot();

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
