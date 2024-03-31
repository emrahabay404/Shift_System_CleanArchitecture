using Shift_System.Application.Extensions;
using Shift_System.Infrastructure.Extensions;
using Shift_System.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);



//builder.Services.AddDbContext<ApplicationDbContext>();
//builder.Services.AddIdentity<AppUser, AppRole>(x =>
//{
//   x.Password.RequireUppercase = false;
//   x.Password.RequireNonAlphanumeric = false;
//})
//   .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddAuthorization();
//builder.Services.AddControllersWithViews();
//builder.Services.AddSession();

//builder.Services.AddMvc(config =>
//{
//   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//   config.Filters.Add(new AuthorizeFilter(policy));
//});
//builder.Services.AddMvc();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
//{
//   //sistemdeki otantikyeyi 15 dk olarak ayarlar.
//   x.ExpireTimeSpan = TimeSpan.FromMinutes(10);
//   x.AccessDeniedPath = new PathString("/User/PageDenied/");
//   x.LoginPath = "/User/Index/";
//});


builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddKendo();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//for identity
//app.UseSession();
//app.UseAuthentication();
//for identity


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
