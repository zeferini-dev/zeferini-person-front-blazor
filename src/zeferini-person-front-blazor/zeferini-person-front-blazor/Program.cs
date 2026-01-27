using zeferini.person.front.blazor.Components;
using zeferini.person.front.blazor.Services;
using MudBlazor.Services;

const string DefaultGatewayUrl = "http://localhost:8084";
const string ApiGatewayUrlKey = "Api:GatewayUrl";
const string ApiCommandUrlKey = "Api:CommandUrl";
const string ApiQueryUrlKey = "Api:QueryUrl";
const string PersonsApiPath = "/api/persons";
const string QueryApiPath = "/api/query";
const string ErrorPagePath = "/Error";
const string NotFoundPagePath = "/not-found";
const string DockerEnvironmentVariable = "DOTNET_RUNNING_IN_CONTAINER";

// Helper method to check if running in Docker container
static bool IsRunningInContainer(WebApplicationBuilder builder)
{
    var configValue = builder.Configuration.GetValue<bool>(DockerEnvironmentVariable, false);
    var envValue = Environment.GetEnvironmentVariable(DockerEnvironmentVariable);
    return configValue || !string.IsNullOrEmpty(envValue);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Adicionar MudBlazor
builder.Services.AddMudServices();

// Configure API via Gateway
builder.Services.Configure<ApiConfiguration>(options =>
{
    var gatewayUrl = builder.Configuration.GetValue<string>(ApiGatewayUrlKey) ?? DefaultGatewayUrl;
    
    options.GatewayUrl = gatewayUrl;
    options.CommandUrl = builder.Configuration.GetValue<string>(ApiCommandUrlKey) ?? $"{gatewayUrl}{PersonsApiPath}";
    options.QueryUrl = builder.Configuration.GetValue<string>(ApiQueryUrlKey) ?? $"{gatewayUrl}{QueryApiPath}";
});

// Adicionar HttpClient e PersonService
builder.Services.AddHttpClient<PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(ErrorPagePath, createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute(NotFoundPagePath, createScopeForStatusCodePages: true);

// Apply HTTPS redirection only if not running in Docker container
if (!IsRunningInContainer(builder))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
