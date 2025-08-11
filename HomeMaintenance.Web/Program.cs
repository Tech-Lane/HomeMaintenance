// Using the components namespace is not required if we fully-qualify below

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HttpClient for API calls
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Disabled for HTTP-only development


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<HomeMaintenance.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
