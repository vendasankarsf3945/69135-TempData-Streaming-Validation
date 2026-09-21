using TempDataStreamingSSR.Components;

var builder = WebApplication.CreateBuilder(args);
var tempDataProviderMode = builder.Configuration["TempDataProvider"] ?? "Cookie";

// Add services to the container.
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Components", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Components.Endpoints", LogLevel.Warning);
builder.Logging.AddFilter("TempDataStreamingSSR.Components", LogLevel.Information);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});
builder.Services.AddRazorComponents(options =>
{
    if (string.Equals(tempDataProviderMode, "Session", StringComparison.OrdinalIgnoreCase))
    {
        options.TempDataProviderType = Microsoft.AspNetCore.Components.Endpoints.TempDataProviderType.SessionStorage;
    }
    else if (string.Equals(tempDataProviderMode, "Cookie", StringComparison.OrdinalIgnoreCase))
    {
        options.TempDataCookie.Name = ".AspNetCore.Components.TempData";
        options.TempDataCookie.HttpOnly = true;
        options.TempDataCookie.SameSite = SameSiteMode.Strict;
        options.TempDataCookie.SecurePolicy = CookieSecurePolicy.None;
    }
    else
    {
        throw new InvalidOperationException($"Unsupported TempDataProvider mode '{tempDataProviderMode}'. Use Cookie or Session.");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseSession();

app.MapStaticAssets();
app.MapRazorComponents<App>();

app.Run();
