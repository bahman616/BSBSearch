using BsbSearch.Data;
using BsbSearch.Infrastructure;
using BsbSearch.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IBsbService, BsbService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("LocalBackendApi", client => client.BaseAddress = new Uri("https://localhost:7287/"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthenticationMiddleware();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapControllers();

app.MapFallbackToPage("/_Host");


app.Run();
