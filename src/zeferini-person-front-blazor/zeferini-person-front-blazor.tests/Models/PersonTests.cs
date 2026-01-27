using FluentAssertions;
using zeferini.person.front.blazor.Models;

namespace zeferini.person.front.blazor.tests.Models;

public class PersonTests
{
    [Fact]
    public void Person_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var person = new Person();

        // Assert
        person.Id.Should().Be(string.Empty);
        person.Name.Should().Be(string.Empty);
        person.Email.Should().Be(string.Empty);
    }

    [Fact]
    public void Person_SetProperties_ShouldSucceed()
    {
        // Arrange
        var person = new Person();
        var id = "123";
        var name = "John Doe";
        var email = "john@example.com";
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddSeconds(10);

        // Act
        person.Id = id;
        person.Name = name;
        person.Email = email;
        person.CreatedAt = createdAt;
        person.UpdatedAt = updatedAt;

        // Assert
        person.Id.Should().Be(id);
        person.Name.Should().Be(name);
        person.Email.Should().Be(email);
        person.CreatedAt.Should().Be(createdAt);
        person.UpdatedAt.Should().Be(updatedAt);
    }

    [Theory]
    [InlineData("1", "Alice", "alice@test.com")]
    [InlineData("2", "Bob", "bob@test.com")]
    [InlineData("3", "Charlie", "charlie@test.com")]
    public void Person_WithVariousValues_ShouldStoreCorrectly(string id, string name, string email)
    {
        // Arrange & Act
        var person = new Person { Id = id, Name = name, Email = email };

        // Assert
        person.Id.Should().Be(id);
        person.Name.Should().Be(name);
        person.Email.Should().Be(email);
    }

    [Fact]
    public void CreatePerson_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var createPerson = new CreatePerson();

        // Assert
        createPerson.Name.Should().Be(string.Empty);
        createPerson.Email.Should().Be(string.Empty);
    }

    [Fact]
    public void CreatePerson_SetProperties_ShouldSucceed()
    {
        // Arrange
        var createPerson = new CreatePerson();
        var name = "Jane Doe";
        var email = "jane@example.com";

        // Act
        createPerson.Name = name;
        createPerson.Email = email;

        // Assert
        createPerson.Name.Should().Be(name);
        createPerson.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("User1", "user1@test.com")]
    [InlineData("User2", "user2@test.com")]
    public void CreatePerson_WithVariousValues_ShouldStoreCorrectly(string name, string email)
    {
        // Arrange & Act
        var createPerson = new CreatePerson { Name = name, Email = email };

        // Assert
        createPerson.Name.Should().Be(name);
        createPerson.Email.Should().Be(email);
    }

    [Fact]
    public void UpdatePerson_DefaultValues_ShouldBeNull()
    {
        // Arrange & Act
        var updatePerson = new UpdatePerson();

        // Assert
        updatePerson.Name.Should().BeNull();
        updatePerson.Email.Should().BeNull();
    }

    [Fact]
    public void UpdatePerson_SetOnlyName_ShouldSucceed()
    {
        // Arrange
        var updatePerson = new UpdatePerson();
        var name = "Updated Name";

        // Act
        updatePerson.Name = name;

        // Assert
        updatePerson.Name.Should().Be(name);
        updatePerson.Email.Should().BeNull();
    }

    [Fact]
    public void UpdatePerson_SetOnlyEmail_ShouldSucceed()
    {
        // Arrange
        var updatePerson = new UpdatePerson();
        var email = "updated@example.com";

        // Act
        updatePerson.Email = email;

        // Assert
        updatePerson.Name.Should().BeNull();
        updatePerson.Email.Should().Be(email);
    }

    [Fact]
    public void UpdatePerson_SetBothProperties_ShouldSucceed()
    {
        // Arrange
        var updatePerson = new UpdatePerson();
        var name = "Updated Name";
        var email = "updated@example.com";

        // Act
        updatePerson.Name = name;
        updatePerson.Email = email;

        // Assert
        updatePerson.Name.Should().Be(name);
        updatePerson.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("Name1", null)]
    [InlineData(null, "email1@test.com")]
    [InlineData("Name2", "email2@test.com")]
    public void UpdatePerson_WithVariousCombinations_ShouldStoreCorrectly(string? name, string? email)
    {
        // Arrange & Act
        var updatePerson = new UpdatePerson { Name = name, Email = email };

        // Assert
        updatePerson.Name.Should().Be(name);
        updatePerson.Email.Should().Be(email);
    }
}
