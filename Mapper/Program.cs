var builder = WebApplication.CreateBuilder(args);

var mapsApiKey = Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY");
var sheetsApiKey = Environment.GetEnvironmentVariable("GOOGLE_SHEETS_API_KEY");

if (!string.IsNullOrEmpty(mapsApiKey))
{
    builder.Configuration["GoogleMaps:ApiKey"] = mapsApiKey;
}

if (!string.IsNullOrEmpty(sheetsApiKey))
{
    builder.Configuration["GoogleSheets:ApiKey"] = sheetsApiKey;
}

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
