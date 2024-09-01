using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Shift_System.Application.Extensions;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Extensions;
using Shift_System_UI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// HttpClient servisini ekleyin
// HttpClient servisini ekleyin
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TokenOptions:BaseApiUrl"]); // API'nin temel adresini buraya yaz�n
    client.DefaultRequestHeaders.Add("Accept", "application/json");

    // �rne�in, burada Bearer token'� da ekleyebilirsiniz, ancak token dinamik olarak de�i�ece�i i�in
    // her bir istek �ncesinde token'� eklemek daha do�ru olur
});


// JWT Authentication i�in gerekli olan servisleri ekleyin
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
//        ValidAudience = builder.Configuration["TokenOptions:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"]))
//    };
//});

// HttpContextAccessor ve Session servisini ekleyin
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Session s�resi
    options.Cookie.HttpOnly = true; // Cookie g�venlik ayarlar�
    options.Cookie.IsEssential = true; // GDPR uyumlulu�u i�in
});
builder.Services.AddScoped<ApiService>();

// Add services to the container with authentication and authorization settings.
builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

// Custom service layer extensions
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

// Configure cookie settings for authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Oturum s�resi: 10 dakika
    options.LoginPath = "/Account/Login/"; // Giri� yolu
    options.AccessDeniedPath = "/Auth/AccessDenied/"; // Eri�im engellendi yolu
    options.SlidingExpiration = true; // Her istekle birlikte s�re uzat�l�r
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Auth/Page404", "?code={0}");
app.UseSession(); // Session middleware'i kullan�ma al�n
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is in the correct order
app.UseAuthorization(); // Authorization middleware should come after authentication

app.MapDefaultControllerRoute();

app.Run();
