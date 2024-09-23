using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using web_clima.Data;
using web_clima.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity options and add EF Core store
builder.Services.AddDefaultIdentity<UserModel>(options =>
{
    // Configure lockout options
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero; // Disables lockout time span
    options.Lockout.MaxFailedAccessAttempts = 0; // Disables lockout after failed attempts
    options.Lockout.AllowedForNewUsers = false; // Disables lockout for new users

    options.SignIn.RequireConfirmedAccount = false; // Require confirmed account for sign-in
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Custom logging middleware for request and response status
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});

app.UseAuthentication();
app.UseAuthorization();

// Redirecionar para o Dashboard como padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure Razor Pages are mapped if used
app.MapRazorPages();

app.Run();
