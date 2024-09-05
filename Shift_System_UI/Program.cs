using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Shift_System.Application.Extensions;
using Shift_System.Application.Interfaces;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Infrastructure.Services;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// HttpClient servisini ekleyin
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TokenOptions:BaseApiUrl"]); // API'nin temel adresini buraya yazýn
    client.DefaultRequestHeaders.Add("Accept", "application/json");

    // Örneðin, burada Bearer token'ý da ekleyebilirsiniz, ancak token dinamik olarak deðiþeceði için
    // her bir istek öncesinde token'ý eklemek daha doðru olur
});

// HttpContextAccessor ve Session servisini ekleyin
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Oturum süresi, 15 dakika
    options.Cookie.HttpOnly = true; // Sadece HTTP istekleriyle eriþim
    options.Cookie.IsEssential = true; // GDPR uyumluluðu için gerekli
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS üzerinden gönder
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
    options.Cookie.SameSite = SameSiteMode.Strict; // CSRF saldýrýlarýna karþý koruma
    options.Cookie.HttpOnly = true; // XSS saldýrýlarýna karþý koruma
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS üzerinden gönder
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // Kýsa tutmak test amaçlýdýr
    options.LoginPath = "/Account/Login/"; // Giriþ yolu
    options.AccessDeniedPath = "/Auth/AccessDenied/"; // Eriþim engellendiði durumda yönlendirilecek yol
    options.SlidingExpiration = true; // Her istekle birlikte süresini uzat
});


builder.Services.AddScoped<IFileUploadService, FileUploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Auth/Page404", "?code={0}");
app.UseSession(); // Session middleware'i kullanýma alýn
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is in the correct order
app.UseAuthorization(); // Authorization middleware should come after authentication

app.MapDefaultControllerRoute();

app.Run();
