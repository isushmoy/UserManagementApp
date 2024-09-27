using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data;
using Microsoft.AspNetCore.Identity;
using UserManagementApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity services

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;  // Disable confirmed account requirement
    options.SignIn.RequireConfirmedEmail = false;    // Disable email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure password options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;               // Remove digit requirement
    options.Password.RequireLowercase = false;           // Remove lowercase requirement
    options.Password.RequireNonAlphanumeric = false;     // Remove special character requirement
    options.Password.RequireUppercase = false;           // Remove uppercase requirement
    options.Password.RequiredLength = 1;                 // Set password length to 1
    options.Password.RequiredUniqueChars = 1;            // Only one unique character required
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "userManagement",
    pattern: "UserManagement/{action=Index}/{id?}",
    defaults: new { controller = "UserManagement" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();
