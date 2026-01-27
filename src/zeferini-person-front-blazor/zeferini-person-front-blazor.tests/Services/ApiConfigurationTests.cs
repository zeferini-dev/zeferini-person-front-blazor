using FluentAssertions;
using zeferini.person.front.blazor.Services;

namespace zeferini.person.front.blazor.tests.Services;

public class ApiConfigurationTests
{
    [Fact]
    public void ApiConfiguration_DefaultValues_ShouldContainDefaultUrls()
    {
        // Arrange & Act
        var config = new ApiConfiguration();

        // Assert
        config.GatewayUrl.Should().Be("http://localhost:8084");
        config.CommandUrl.Should().Be("http://localhost:8084/api/persons");
        config.QueryUrl.Should().Be("http://localhost:8084/api/query");
    }

    [Fact]
    public void ApiConfiguration_SetGatewayUrl_ShouldSucceed()
    {
        // Arrange
        var config = new ApiConfiguration();
        var newGatewayUrl = "http://api.example.com";

        // Act
        config.GatewayUrl = newGatewayUrl;

        // Assert
        config.GatewayUrl.Should().Be(newGatewayUrl);
    }

    [Fact]
    public void ApiConfiguration_SetCommandUrl_ShouldSucceed()
    {
        // Arrange
        var config = new ApiConfiguration();
        var newCommandUrl = "http://api.example.com/commands";

        // Act
        config.CommandUrl = newCommandUrl;

        // Assert
        config.CommandUrl.Should().Be(newCommandUrl);
    }

    [Fact]
    public void ApiConfiguration_SetQueryUrl_ShouldSucceed()
    {
        // Arrange
        var config = new ApiConfiguration();
        var newQueryUrl = "http://api.example.com/queries";

        // Act
        config.QueryUrl = newQueryUrl;

        // Assert
        config.QueryUrl.Should().Be(newQueryUrl);
    }

    [Fact]
    public void ApiConfiguration_SetAllUrls_ShouldSucceed()
    {
        // Arrange
        var config = new ApiConfiguration();
        var gatewayUrl = "http://gateway.example.com";
        var commandUrl = "http://gateway.example.com/commands";
        var queryUrl = "http://gateway.example.com/queries";

        // Act
        config.GatewayUrl = gatewayUrl;
        config.CommandUrl = commandUrl;
        config.QueryUrl = queryUrl;

        // Assert
        config.GatewayUrl.Should().Be(gatewayUrl);
        config.CommandUrl.Should().Be(commandUrl);
        config.QueryUrl.Should().Be(queryUrl);
    }

    [Theory]
    [InlineData("http://localhost:9000", "http://localhost:9000/api/persons", "http://localhost:9000/api/query")]
    [InlineData("http://api.prod.com", "http://api.prod.com/api/persons", "http://api.prod.com/api/query")]
    [InlineData("http://192.168.1.1:8080", "http://192.168.1.1:8080/api/persons", "http://192.168.1.1:8080/api/query")]
    public void ApiConfiguration_WithVariousUrls_ShouldStoreCorrectly(string gateway, string command, string query)
    {
        // Arrange & Act
        var config = new ApiConfiguration
        {
            GatewayUrl = gateway,
            CommandUrl = command,
            QueryUrl = query
        };

        // Assert
        config.GatewayUrl.Should().Be(gateway);
        config.CommandUrl.Should().Be(command);
        config.QueryUrl.Should().Be(query);
    }

    [Fact]
    public void ApiConfiguration_UpdateUrl_ShouldReplaceOldValue()
    {
        // Arrange
        var config = new ApiConfiguration();
        var oldUrl = config.GatewayUrl;
        var newUrl = "http://newgateway.com";

        // Act
        config.GatewayUrl = newUrl;

        // Assert
        config.GatewayUrl.Should().NotBe(oldUrl);
        config.GatewayUrl.Should().Be(newUrl);
    }

    [Fact]
    public void ApiConfiguration_WithEmptyStrings_ShouldStoreEmpty()
    {
        // Arrange & Act
        var config = new ApiConfiguration
        {
            GatewayUrl = string.Empty,
            CommandUrl = string.Empty,
            QueryUrl = string.Empty
        };

        // Assert
        config.GatewayUrl.Should().Be(string.Empty);
        config.CommandUrl.Should().Be(string.Empty);
        config.QueryUrl.Should().Be(string.Empty);
    }
}
