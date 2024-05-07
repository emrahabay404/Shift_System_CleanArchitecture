using Shift_System.Application.Extensions;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddJwtAuthentication();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
public partial class Program { }