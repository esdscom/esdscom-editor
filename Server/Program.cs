using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;

Log.Logger = new LoggerConfiguration()
              .WriteTo.File(@"logs/authortool.log", rollingInterval: RollingInterval.Day)
              .CreateLogger();

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

Log.Information("Application started");
