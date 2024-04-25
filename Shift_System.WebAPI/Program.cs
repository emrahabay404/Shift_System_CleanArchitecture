using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Extensions;
using Shift_System.Domain.Entities;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Contexts;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//bu olmadan login register olmuyor
//builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Shift_Db_Conn")));
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

//bu satýrý aþaðýya koymammýn nedeni, jwt authenticationu görmesi için
builder.Services.AddPersistenceLayer(builder.Configuration);


//builder.Services.AddJwtAuthentication();

builder.Services.AddControllers();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
   app.UseSwagger();
   app.UseSwaggerUI();
}

//for jwt
app.UseAuthorization();

app.MapControllers();
app.Run();


