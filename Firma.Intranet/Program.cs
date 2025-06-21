using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Firma.Data.Data;
using System.Text.Json.Serialization;
using Firma.Data.Data.Customers;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FirmaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirmaContext") ?? throw new InvalidOperationException("Connection string 'FirmaContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Mówie serializerowi, żeby ignorował cykliczne odwołania zamiast zawieszać aplikację
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Konfiguracja uwierzytelniania za pomocą ciasteczek
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Czas życia ciasteczka
        options.LoginPath = "/Account/Login"; // Ścieżka do strony logowania
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ścieżka przy braku uprawnień
        options.SlidingExpiration = true;
    });

// Konfiguracja autoryzacji opartej na rolach
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(UserRole.Admin.ToString()));
    options.AddPolicy("AdminOrModerator", policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.Moderator.ToString()));
});


//Ustawienie licencji biblioteki QuestPDF na community
QuestPDF.Settings.License = LicenseType.Community;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
