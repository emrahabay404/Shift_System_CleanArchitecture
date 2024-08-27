using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Shift_System.Application.Extensions;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    //sistemdeki otantikyeyi 100 dk olarak ayarlar.
    x.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    x.AccessDeniedPath = new PathString("/Login/PageDenied/");
    x.LoginPath = "/Home/Index/";
});

builder.Services.AddMvc();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Login/Page404", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
