using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCoreAdmin();
// !!! I will change this after development phase. !!!
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "shoppingCart",
        pattern: "ShoppingCart",
        defaults: new { controller = "ShoppingCart", action = "Index" }
    );
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Manager", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    //await context.Database.MigrateAsync();
    //await SeedData.SeedRolesAsync(roleManager);
    //await SeedData.SeedSuperAdminAsync(userManager);
}

using (var scope = app.Services.CreateScope())
{
    var userManager =
scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";
    string password = "Admin123!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();


//public class Program
//{

//    public static async Task Main(string[] args)
//    {

//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//        builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            options.UseSqlServer(connectionString));
//        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//        // !!! I will change this after development phase. !!!
//        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//            .AddRoles<IdentityRole>()
//            .AddEntityFrameworkStores<ApplicationDbContext>();
//        builder.Services.AddControllersWithViews();

//        builder.Services.AddCoreAdmin();


//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseMigrationsEndPoint();
//        }
//        else
//        {
//            app.UseExceptionHandler("/Home/Error");
//            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//            app.UseHsts();
//        }

//        app.UseHttpsRedirection();
//        app.UseStaticFiles();

//        app.UseRouting();

//        app.UseAuthorization();
//        app.MapRazorPages();

//        app.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}");


//        //app.MapControllerRoute(
//        //        name: "shoppingCart",
//        //        pattern: "ShoppingCart",
//        //        defaults: new { controller = "ShoppingCart", action = "Index" }
//        //    );

//        using (var scope = app.Services.CreateScope())
//        {
//            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//            var roles = new[] { "Admin", "Manager", "Member" };
//            foreach (var role in roles)
//            {
//                if (!await roleManager.RoleExistsAsync(role))
//                {
//                    await roleManager.CreateAsync(new IdentityRole(role));
//                }
//            }
//            //await context.Database.MigrateAsync();
//            //await SeedData.SeedRolesAsync(roleManager);
//            //await SeedData.SeedSuperAdminAsync(userManager);
//        }

//        using (var scope = app.Services.CreateScope())
//        {
//            var userManager =
//        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//            string email = "admin@admin.com";
//            string password = "Admin123!";

//            if (await userManager.FindByEmailAsync(email) == null)
//            {
//                var user = new IdentityUser();
//                user.UserName = email;
//                user.Email = email;

//                await userManager.CreateAsync(user, password);

//                await userManager.AddToRoleAsync(user, "Admin");
//            }
//        }

//        app.Run();




//    }

//}

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddRoles<IdentityRole>() // Rolleri aktif etmek i�in
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddControllersWithViews();

//builder.Services.AddCoreAdmin();
//builder.Services.AddScoped<ShoppingCart>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//        name: "shoppingCart",
//        pattern: "ShoppingCart",
//        defaults: new { controller = "ShoppingCart", action = "Index" }
//    );

//app.MapRazorPages();

//app.Run();




