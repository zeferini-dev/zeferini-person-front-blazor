using first.Components;
using first.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Adicionar MudBlazor
builder.Services.AddMudServices();

// Configurar API
builder.Services.Configure<ApiConfiguration>(options =>
{
    options.CommandUrl = builder.Configuration.GetValue<string>("Api:CommandUrl") ?? "http://localhost:3000";
    options.QueryUrl = builder.Configuration.GetValue<string>("Api:QueryUrl") ?? "http://localhost:3001";
});

// Adicionar HttpClient e PersonService
builder.Services.AddHttpClient<PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Não usar HTTPS redirection em container Docker
if (!builder.Configuration.GetValue<bool>("DOTNET_RUNNING_IN_CONTAINER", false) 
    && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
