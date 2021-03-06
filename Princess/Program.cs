using Microsoft.EntityFrameworkCore;
using Princess.Bot;
using Princess.Data;
using Princess.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRazorPages();
builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<PresenceHandler>();
builder.Services.AddScoped<TriviaQuestions>();

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

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider
        .GetRequiredService<DbService>();

    if (app.Environment.IsProduction()) await ctx.IsCreatedAsync();
    if (app.Environment.IsDevelopment()) await ctx.RecreateAsync();
}

var bot = new Bot(app.Services);

bot.RunAsync().GetAwaiter();

app.Run();