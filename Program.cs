using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using moore.Data;
using moore.Models;
using System;
// The following services are added to the container:
var builder = WebApplication.CreateBuilder(args);
// DbContext<mooreContext> is added and configured to use SQL Server with a connection string named "mooreContext".
builder.Services.AddDbContext<mooreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mooreContext") ?? throw new InvalidOperationException("Connection string 'mooreContext' not found."));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// - Identity is added and configured with the ApplicationUser model and IdentityRole as the default role.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<mooreContext>()
                .AddDefaultTokenProviders();
//Authorization policies are added for "PaidAccess" and "UnpaidAccess" requiring a claim with SubscriptionType value of "Paid" or "Unpaid", respectively.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PaidAccess", policy =>
        policy.RequireClaim("SubscriptionType", "Paid"));
    options.AddPolicy("UnpaidAccess", policy =>
        policy.RequireClaim("SubscriptionType", "Unpaid"));
});
// The app is built and a scope is created to seed the database with data from Datainit class.

var app = builder.Build();
// for data testing seeddata 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    //required for login and registration cus i use another method to seed the login info 
    // this one was normal so no need await 
    Datainit.Initialize(services);
    //auto migrate dont need to migrate everytime 
    
    var context = services.GetRequiredService<mooreContext>();

    context.Database.Migrate();



}
//SeedDatabase ---------------------------------------------------------------------- using async method should be called like this await 
await Datainit.SeedingUsersAndRolesAsync(app);

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
// this should be used for login and registeration
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}",
    constraints: new { id = "[A-Za-z0-9-]+" });

app.Run();
