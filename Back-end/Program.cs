using Backend.Models;
using Backend.Models.DbModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DatabaseContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SL-Connection")));

builder.Services.AddRazorPages();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options =>
//        {
//            options.LoginPath = new PathString("/auth/login");
//            options.AccessDeniedPath = new PathString("/auth/denied");
//        });

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BackendContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// adding data to db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedModel.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Offer}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Offer}/{action=Index}/{id?}");

app.Run();
