using FluentAssertions;
using Moq;
using System.Net;
using Microsoft.Extensions.Options;
using zeferini.person.front.blazor.Models;
using zeferini.person.front.blazor.Services;

namespace zeferini.person.front.blazor.tests.Services;

public class PersonServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IOptions<ApiConfiguration>> _apiConfigMock;
    private readonly ApiConfiguration _apiConfiguration;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://localhost") };
        
        _apiConfiguration = new ApiConfiguration
        {
            GatewayUrl = "http://localhost:8084",
            CommandUrl = "http://localhost:8084/api/persons",
            QueryUrl = "http://localhost:8084/api/query"
        };
        
        _apiConfigMock = new Mock<IOptions<ApiConfiguration>>();
        _apiConfigMock.Setup(x => x.Value).Returns(_apiConfiguration);
        
        _personService = new PersonService(_httpClient, _apiConfigMock.Value);
    }

    [Fact]
    public void PersonService_Constructor_ShouldInitialize()
    {
        // Act & Assert
        _personService.Should().NotBeNull();
    }

    [Fact]
    public async Task ListAsync_WithValidResponse_ShouldReturnPersonList()
    {
        // Arrange
        var expectedPersons = new List<Person>
        {
            new Person { Id = "1", Name = "Person 1", Email = "person1@test.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Person { Id = "2", Name = "Person 2", Email = "person2@test.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(expectedPersons);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _personService.ListAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Person 1");
        result[1].Name.Should().Be("Person 2");
    }

    [Fact]
    public async Task ListAsync_WithEmptyResponse_ShouldReturnEmptyList()
    {
        // Arrange
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new List<Person>());
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _personService.ListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ListAsync_WithNullResponse_ShouldReturnEmptyList()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _personService.ListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_WithValidId_ShouldReturnPerson()
    {
        // Arrange
        var personId = "123";
        var expectedPerson = new Person { Id = personId, Name = "Test Person", Email = "test@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(expectedPerson);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _personService.GetAsync(personId);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(personId);
        result?.Name.Should().Be("Test Person");
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _personService.GetAsync("invalid-id");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnCreatedPerson()
    {
        // Arrange
        var createData = new CreatePerson { Name = "New Person", Email = "new@example.com" };
        var expectedPerson = new Person { Id = "123", Name = "New Person", Email = "new@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(expectedPerson);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var dataChangedCalled = false;
        _personService.OnDataChanged += () => dataChangedCalled = true;

        // Act
        var result = await _personService.CreateAsync(createData);

        // Assert
        result.Should().NotBeNull();
        result?.Name.Should().Be("New Person");
        result?.Email.Should().Be("new@example.com");
        dataChangedCalled.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_WithFailedResponse_ShouldThrowException()
    {
        // Arrange
        var createData = new CreatePerson { Name = "New Person", Email = "new@example.com" };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _personService.CreateAsync(createData));
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldReturnUpdatedPerson()
    {
        // Arrange
        var personId = "123";
        var updateData = new UpdatePerson { Name = "Updated Name", Email = "updated@example.com" };
        var expectedPerson = new Person { Id = personId, Name = "Updated Name", Email = "updated@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(expectedPerson);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var dataChangedCalled = false;
        _personService.OnDataChanged += () => dataChangedCalled = true;

        // Act
        var result = await _personService.UpdateAsync(personId, updateData);

        // Assert
        result.Should().NotBeNull();
        result?.Name.Should().Be("Updated Name");
        result?.Email.Should().Be("updated@example.com");
        dataChangedCalled.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_WithFailedResponse_ShouldThrowException()
    {
        // Arrange
        var personId = "123";
        var updateData = new UpdatePerson { Name = "Updated Name" };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _personService.UpdateAsync(personId, updateData));
    }

    [Fact]
    public async Task RemoveAsync_WithValidId_ShouldSucceed()
    {
        // Arrange
        var personId = "123";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var dataChangedCalled = false;
        _personService.OnDataChanged += () => dataChangedCalled = true;

        // Act
        await _personService.RemoveAsync(personId);

        // Assert
        dataChangedCalled.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveAsync_WithFailedResponse_ShouldThrowException()
    {
        // Arrange
        var personId = "invalid";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _personService.RemoveAsync(personId));
    }

    [Fact]
    public void NotifyDataChanged_WithSubscribedEvent_ShouldInvokeEvent()
    {
        // Arrange
        var eventCalled = false;
        _personService.OnDataChanged += () => eventCalled = true;

        // Act
        _personService.NotifyDataChanged();

        // Assert
        eventCalled.Should().BeTrue();
    }

    [Fact]
    public void NotifyDataChanged_WithMultipleSubscribers_ShouldInvokeAllEvents()
    {
        // Arrange
        var callCount = 0;
        _personService.OnDataChanged += () => callCount++;
        _personService.OnDataChanged += () => callCount++;
        _personService.OnDataChanged += () => callCount++;

        // Act
        _personService.NotifyDataChanged();

        // Assert
        callCount.Should().Be(3);
    }

    [Fact]
    public void NotifyDataChanged_WithoutSubscribers_ShouldNotThrow()
    {
        // Act & Assert
        var action = () => _personService.NotifyDataChanged();
        action.Should().NotThrow();
    }
}
