using RedisExchangeApi.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<RedisService>();



var app = builder.Build();



using (var serviceScope = app.Services.CreateScope())

{

    var services = serviceScope.ServiceProvider;

    var redisService = services.GetRequiredService<RedisService>();

    redisService.Connect();

}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
