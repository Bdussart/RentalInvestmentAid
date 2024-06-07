using Microsoft.AspNetCore.Components;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();

IDatabaseFactory databaseFactory = new SqlServerDatabase();
builder.Services.AddCascadingValue(sp =>
{
    CachingManager cachingManager = new CachingManager(databaseFactory);
    var source = new CascadingValueSource<CachingManager>("CachingManager", cachingManager, false);

    

    return source;
});

builder.Services.AddCascadingValue(sp =>
{
    var source = new CascadingValueSource<IDatabaseFactory>("DatabaseFactory", databaseFactory, false);
    return source;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


//Design pattern for Blazor : https://medium.com/it-dead-inside/lets-learn-blazor-fluxor-app-state-for-blazor-422194eeac26