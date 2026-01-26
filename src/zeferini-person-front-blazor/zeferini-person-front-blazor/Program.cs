using zeferini.person.front.blazor..Components;
using first.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Adicionar MudBlazor
builder.Services.AddMudServices();

// Configurar API via Gateway
builder.Services.Configure<ApiConfiguration>(options =>
{
    var gatewayUrl = builder.Configuration.GetValue<string>("Api:GatewayUrl") ?? "http://localhost:8084";
    options.GatewayUrl = gatewayUrl;
    options.CommandUrl = builder.Configuration.GetValue<string>("Api:CommandUrl") ?? $"{gatewayUrl}/api/persons";
    options.QueryUrl = builder.Configuration.GetValue<string>("Api:QueryUrl") ?? $"{gatewayUrl}/api/query";
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
