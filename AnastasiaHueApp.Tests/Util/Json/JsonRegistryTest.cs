using System.Text.Json;
using AnastasiaHueApp.Util.Json;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Json;

[TestClass]
[TestSubject(typeof(JsonRegistry))]
public class JsonRegistryTest
{
    #region TestClasses

    private class Passport()
    {
        public required string FullName { get; init; }
        public required string Nationality { get; init; }
        public int Age { get; init; }
    }

    private class DriverLicense()
    {
        public required string Type { get; init; }
    }

    #endregion

    [TestMethod]
    public void Parse_Passport_MatchingValues()
    {
        // Arrange.
        var registry = new JsonRegistry();
        registry.Register<Passport>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return new Passport
            {
                FullName = doc.RootElement.GetProperty("FullName").GetString()!,
                Nationality = doc.RootElement.GetProperty("Nationality").GetString()!,
                Age = doc.RootElement.GetProperty("Age").GetInt32(),
            };
        });

        // Act.
        string passportString = """
                                {
                                  "FullName": "John Doe",
                                  "Nationality": "American",
                                  "Age": 34
                                }
                                """;
        var passport = registry.Parse<Passport>(passportString);

        // Assert.
        passport.FullName.Should().Be("John Doe");
        passport.Nationality.Should().Be("American");
        passport.Age.Should().Be(34);
    }

    [TestMethod]
    public void Parse_Passport_InvalidString_ReturnsNull()
    {
        // Arrange.
        var registry = new JsonRegistry();
        registry.Register<Passport>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return new Passport
            {
                FullName = doc.RootElement.GetProperty("FullName").GetString()!,
                Nationality = doc.RootElement.GetProperty("Nationality").GetString()!,
                Age = doc.RootElement.GetProperty("Age").GetInt32(),
            };
        });

        // Act.
        string jsonString = """
                                {
                                  "FavouriteFood": "Pizza",
                                  "BestFootballer": "Cristiano Ronaldo",
                                  "FavouriteChildhoodGame": "Minecraft"
                                }
                                """;
        var passport = registry.Parse<Passport>(jsonString);

        // Assert.
        passport.Should().BeNull("the json string was incorrect, thus it is not of this type of object");
    }
}