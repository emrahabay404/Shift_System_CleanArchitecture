using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Shift_System.Application.Extensions;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); // HttpClient servisini ekle

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
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Oturum süresi: 10 dakika
    options.LoginPath = "/Account/Login"; // Giriþ yolu
    options.AccessDeniedPath = "/Auth/AccessDenied"; // Eriþim engellendi yolu
    options.SlidingExpiration = true; // Her istekle birlikte süre uzatýlýr
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is in the correct order
app.UseAuthorization(); // Authorization middleware should come after authentication

app.MapDefaultControllerRoute();

app.Run();









//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Shift_System.Application.Extensions;
//using Shift_System.Infrastructure.Extensions;
//using Shift_System.Persistence.Extensions;
//var builder = WebApplication.CreateBuilder(args);
//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddSession();
//builder.Services.AddApplicationLayer();
//builder.Services.AddInfrastructureLayer();
//builder.Services.AddPersistenceLayer(builder.Configuration);
//// Kimlik doðrulama için cookie ayarlarýný yapýlandýrýn
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Oturum süresi: 60 dakika
//    options.LoginPath = "/Account/Login"; // Giriþ yolu
//    options.AccessDeniedPath = "/Account/AccessDenied"; // Eriþim engellendi yolu
//    options.SlidingExpiration = true; // Her istekle birlikte süre uzatýlýr
//});
//builder.Services.AddMvc(config =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    config.Filters.Add(new AuthorizeFilter(policy));
//});
//var app = builder.Build();
//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//app.UseStatusCodePagesWithReExecute("/Login/Page404", "?code={0}");
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.MapDefaultControllerRoute();
//app.UseSession();  // Ensure session is used before authentication
//app.UseRouting();
//app.UseAuthentication(); // Ensure authentication middleware is in the correct order
//app.UseAuthorization(); // Authorization middleware should come after authentication
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.Run();
