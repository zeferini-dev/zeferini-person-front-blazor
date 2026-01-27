using FluentAssertions;
using Moq;
using Microsoft.Extensions.Options;
using zeferini.person.front.blazor.Models;
using zeferini.person.front.blazor.Services;

namespace zeferini.person.front.blazor.tests.Services;

public class PersonServiceTests
{
    private readonly Mock<HttpClient> _httpClientMock;
    private readonly Mock<IOptions<ApiConfiguration>> _apiConfigMock;
    private readonly ApiConfiguration _apiConfiguration;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _httpClientMock = new Mock<HttpClient>();
        
        _apiConfiguration = new ApiConfiguration
        {
            GatewayUrl = "http://localhost:8084",
            CommandUrl = "http://localhost:8084/api/persons",
            QueryUrl = "http://localhost:8084/api/query"
        };
        
        _apiConfigMock = new Mock<IOptions<ApiConfiguration>>();
        _apiConfigMock.Setup(x => x.Value).Returns(_apiConfiguration);
        
        _personService = new PersonService(_httpClientMock.Object, _apiConfigMock.Object);
    }

    [Fact]
    public void PersonService_Constructor_ShouldInitialize()
    {
        // Act & Assert
        _personService.Should().NotBeNull();
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

    [Fact]
    public void OnDataChanged_Event_CanBeSubscribedAndUnsubscribed()
    {
        // Arrange
        var eventCalled = false;
        Action handler = () => eventCalled = true;

        // Act - Subscribe
        _personService.OnDataChanged += handler;
        _personService.NotifyDataChanged();
        var afterSubscribe = eventCalled;

        // Reset and unsubscribe
        eventCalled = false;
        _personService.OnDataChanged -= handler;
        _personService.NotifyDataChanged();
        var afterUnsubscribe = eventCalled;

        // Assert
        afterSubscribe.Should().BeTrue();
        afterUnsubscribe.Should().BeFalse();
    }

    [Fact]
    public void PersonService_ApiConfigurationProperties_ShouldBeDefined()
    {
        // Arrange & Act - The configuration is injected in constructor

        // Assert
        _apiConfiguration.GatewayUrl.Should().Be("http://localhost:8084");
        _apiConfiguration.CommandUrl.Should().Be("http://localhost:8084/api/persons");
        _apiConfiguration.QueryUrl.Should().Be("http://localhost:8084/api/query");
    }
}
