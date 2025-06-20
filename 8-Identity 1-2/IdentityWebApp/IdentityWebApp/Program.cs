using IdentityWebApp.Models;
using Microsoft.EntityFrameworkCore;
using IdentityWebApp.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityWebApp.Extensions;
using IdentityWebApp.Models;
using IdentityWebApp.OptionsModels;
using IdentityWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using IdentityWebApp.ClaimProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreIdentityApp.Web.Requirements;
using AspNetCoreIdentityApp.Web.PermissionsRoot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentityWithExt();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");


    });

    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());


    });


    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });


    });
    options.AddPolicy("OrderPermissionReadAndDelete", policy =>
    {

        policy.RequireClaim("permission", Permissions.Order.Read);
        policy.RequireClaim("permission", Permissions.Order.Delete);
        policy.RequireClaim("permission", Permissions.Stock.Delete);

    });



    options.AddPolicy("Permissions.Order.Read", policy =>
    {

        policy.RequireClaim("permission", Permissions.Order.Read);


    });

    options.AddPolicy("Permissions.Order.Delete", policy =>
    {

        policy.RequireClaim("permission", Permissions.Order.Delete);


    });


    options.AddPolicy("Permissions.Stock.Delete", policy =>
    {

        policy.RequireClaim("permission", Permissions.Stock.Delete);


    });

});

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "UdemyAppCookie";
    opt.LoginPath = new PathString("/Home/Signin");
    opt.LogoutPath = new PathString("/Member/logout");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();