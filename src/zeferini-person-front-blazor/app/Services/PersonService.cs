using System.Net.Http.Json;
using first.Models;
using Microsoft.Extensions.Options;

namespace first.Services;

/// <summary>
/// Serviço para gerenciar operações CRUD de Pessoa
/// Implementa CQRS via API Gateway: Commands → Load Balanced, Queries → MongoDB
/// </summary>
public class PersonService
{
    private readonly HttpClient _httpClient;
    private readonly ApiConfiguration _apiConfig;
    // Command URL já inclui /api/persons, Query URL inclui /api/query
    private string CommandResource => _apiConfig.CommandUrl;
    private string QueryResource => $"{_apiConfig.QueryUrl}/persons";

    // Evento para notificar que os dados foram alterados
    public event Action? OnDataChanged;

    public PersonService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfig)
    {
        _httpClient = httpClient;
        _apiConfig = apiConfig.Value;
    }

    /// <summary>
    /// Lista todas as pessoas (QUERY → MongoDB)
    /// </summary>
    public async Task<List<Person>> ListAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Person>>(QueryResource);
        return result ?? [];
    }

    /// <summary>
    /// Busca uma pessoa por ID (QUERY → MongoDB)
    /// </summary>
    public async Task<Person?> GetAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Person>($"{QueryResource}/{id}");
    }

    /// <summary>
    /// Cria uma nova pessoa (COMMAND → MySQL)
    /// </summary>
    public async Task<Person?> CreateAsync(CreatePerson data)
    {
        var response = await _httpClient.PostAsJsonAsync(CommandResource, data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Person>();
        NotifyDataChanged();
        return result;
    }

    /// <summary>
    /// Atualiza uma pessoa existente (COMMAND → MySQL)
    /// </summary>
    public async Task<Person?> UpdateAsync(string id, UpdatePerson data)
    {
        var response = await _httpClient.PatchAsJsonAsync($"{CommandResource}/{id}", data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Person>();
        NotifyDataChanged();
        return result;
    }

    /// <summary>
    /// Remove uma pessoa (COMMAND → MySQL)
    /// </summary>
    public async Task RemoveAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"{CommandResource}/{id}");
        response.EnsureSuccessStatusCode();
        NotifyDataChanged();
    }

    /// <summary>
    /// Notifica que os dados foram alterados
    /// </summary>
    public void NotifyDataChanged() => OnDataChanged?.Invoke();
}
