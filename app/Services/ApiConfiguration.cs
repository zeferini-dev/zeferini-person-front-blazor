namespace first.Services;

/// <summary>
/// Configuração das URLs das APIs
/// </summary>
public class ApiConfiguration
{
    /// <summary>
    /// API de escrita (comandos): MySQL - porta 3000
    /// </summary>
    public string CommandUrl { get; set; } = "http://localhost:3000";

    /// <summary>
    /// API de leitura (queries): MongoDB - porta 3001
    /// </summary>
    public string QueryUrl { get; set; } = "http://localhost:3001";
}
