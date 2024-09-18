using JerseyShop.Data;  // Assicurati che ci sia il namespace corretto
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Configurazione Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura il DbContext con SQL Server
builder.Services.AddDbContext<FootballShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura Identity per gestire l'autenticazione e i ruoli
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;  // Disabilita la conferma email
    options.Lockout.AllowedForNewUsers = false;      // Disabilita il lockout per nuovi utenti
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FootballShopContext>();

// Aggiungi supporto per la cache e la sessione
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(FavoritesController).Assembly);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Imposta il timeout della sessione
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Seed dell'admin predefinito
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.CreateDefaultAdmin(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();  // Usa la sessione

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.MapRazorPages();

app.Run();
