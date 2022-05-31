using Microsoft.EntityFrameworkCore;
using Princess.Bot;
using Princess.CSV;
using Princess.Data;
using Princess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<PresenceHandler>();

builder.Services.AddDbContext<PresenceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

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

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider
        .GetRequiredService<DbService>();

    if (app.Environment.IsProduction()) await ctx.IsCreatedAsync();
    if (app.Environment.IsDevelopment()) await ctx.RecreateAsync();
}

var bot = new Bot(app.Services);

bot.RunAsync().GetAwaiter();

var csvCreateFile = new CsvProgram();

app.Run();