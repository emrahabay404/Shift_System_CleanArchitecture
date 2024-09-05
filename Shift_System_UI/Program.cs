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
    client.BaseAddress = new Uri(builder.Configuration["TokenOptions:BaseApiUrl"]); // API'nin temel adresini buraya yaz�n
    client.DefaultRequestHeaders.Add("Accept", "application/json");

    // �rne�in, burada Bearer token'� da ekleyebilirsiniz, ancak token dinamik olarak de�i�ece�i i�in
    // her bir istek �ncesinde token'� eklemek daha do�ru olur
});

// HttpContextAccessor ve Session servisini ekleyin
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Oturum s�resi, 15 dakika
    options.Cookie.HttpOnly = true; // Sadece HTTP istekleriyle eri�im
    options.Cookie.IsEssential = true; // GDPR uyumlulu�u i�in gerekli
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS �zerinden g�nder
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
    options.Cookie.SameSite = SameSiteMode.Strict; // CSRF sald�r�lar�na kar�� koruma
    options.Cookie.HttpOnly = true; // XSS sald�r�lar�na kar�� koruma
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Sadece HTTPS �zerinden g�nder
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // K�sa tutmak test ama�l�d�r
    options.LoginPath = "/Account/Login/"; // Giri� yolu
    options.AccessDeniedPath = "/Auth/AccessDenied/"; // Eri�im engellendi�i durumda y�nlendirilecek yol
    options.SlidingExpiration = true; // Her istekle birlikte s�resini uzat
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
app.UseSession(); // Session middleware'i kullan�ma al�n
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is in the correct order
app.UseAuthorization(); // Authorization middleware should come after authentication

app.MapDefaultControllerRoute();

app.Run();
