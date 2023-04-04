global using Infastructure.Identity;
global using ApplicationCore.Interfaces;
global using ApplicationCore.Entities;
global using Web.Models;
global using Web.Extensions;
global using Web.Interfaces;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Web.Services;
using ApplicationCore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ShopContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ShopContext")));
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AppIdentityDbContext");
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
builder.Services.AddScoped<IHomeViewModelService,HomeViewModelService>();
builder.Services.AddScoped<IBasketViewModelService, BasketViewModelService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

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
app.UseRequestLocalization("en-US");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using(var scope = app.Services.CreateScope())
{
    var shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>();
    var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await ShopContextSeed.SeedAsync(shopContext);
    await AppIdentityDbContextSeed.SeedAsync(identityContext,roleManager, userManager);
}
app.Run();
