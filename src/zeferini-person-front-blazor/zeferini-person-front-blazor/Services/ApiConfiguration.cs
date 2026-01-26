namespace zeferini.person.front.blazor.Services;

/// <summary>
/// Configuração das URLs das APIs via Gateway
/// </summary>
public class ApiConfiguration
{
    /// <summary>
    /// URL base do API Gateway
    /// </summary>
    public string GatewayUrl { get; set; } = "http://localhost:8084";

    /// <summary>
    /// API de escrita (comandos): via Gateway -> Load Balanced
    /// </summary>
    public string CommandUrl { get; set; } = "http://localhost:8084/api/persons";

    /// <summary>
    /// API de leitura (queries): via Gateway -> MongoDB
    /// </summary>
    public string QueryUrl { get; set; } = "http://localhost:8084/api/query";
}
