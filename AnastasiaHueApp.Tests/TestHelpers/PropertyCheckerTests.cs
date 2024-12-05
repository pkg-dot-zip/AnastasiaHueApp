using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.TestHelpers;

[TestClass]
[TestSubject(typeof(PropertyChecker))]
public class PropertyCheckerTests
{
    #region TestClasses

    private class BeautifulTestClass1()
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
    }

    #endregion

    #region CheckAllPropertiesAreNotNull

    #region ReturnsValue

    [TestMethod]
    [DataRow("", "", "")]
    [DataRow("Kacper", "Duda", "Polish U-21 Footballer")]
    public void CheckAllPropertiesAreNotNull_CheckTestClass1AllPropertiesNotNull_ReturnsTrue(string firstName,
        string lastName, string description)
    {
        // Arrange.
        var testObj = new BeautifulTestClass1
        {
            FirstName = firstName,
            LastName = lastName,
            Description = description,
        };

        // Act & Assert.
        PropertyChecker.CheckAllPropertiesAreNotNull(testObj, out _).Should().BeTrue();
    }

    [TestMethod]
    public void CheckAllPropertiesAreNotNull_CheckTestClass1AllPropertiesNull_ReturnsFalse()
    {
        // Arrange.
        var testObj = new BeautifulTestClass1
        {
            FirstName = null,
            LastName = null,
            Description = null,
        };

        // Act & Assert.
        PropertyChecker.CheckAllPropertiesAreNotNull(testObj, out _).Should().BeFalse();
    }

    #endregion

    #region OutDictionaryCorrect

    [TestMethod]
    [DataRow("", "", "")]
    [DataRow("Patryk", "Peda", "U-21 Polish Footballer")]
    public void CheckAllPropertiesAreNotNull_CheckTestClass1PropertiesDictionaryCheckCorrectValue_ReturnsTrue(string? firstName, string? lastName, string? description)
    {
        // Arrange.
        var testObj = new BeautifulTestClass1
        {
            FirstName = firstName,
            LastName = lastName,
            Description = description,
        };

        // Act.
        PropertyChecker.CheckAllPropertiesAreNotNull(testObj, out var dic);

        // Assert.
        dic[nameof(BeautifulTestClass1.FirstName)].Should().BeTrue();
        dic[nameof(BeautifulTestClass1.LastName)].Should().BeTrue();
        dic[nameof(BeautifulTestClass1.Description)].Should().BeTrue();
    }

    [TestMethod]
    [DataRow("", "", null)]
    [DataRow(null, null, null)]
    [DataRow("Patryk", null, null)]
    [DataRow("Patryk", "Peda", null)]
    public void CheckAllPropertiesAreNotNull_CheckTestClass1PropertiesDictionaryCheckCorrectValue_ReturnsFalse(string? firstName, string? lastName, string? description)
    {
        // Arrange.
        var testObj = new BeautifulTestClass1
        {
            FirstName = firstName,
            LastName = lastName,
            Description = description,
        };

        // Act.
        PropertyChecker.CheckAllPropertiesAreNotNull(testObj, out var dic);

        // Assert.
        dic[nameof(BeautifulTestClass1.FirstName)].Should().Be(firstName is not null);
        dic[nameof(BeautifulTestClass1.LastName)].Should().Be(lastName is not null);
        dic[nameof(BeautifulTestClass1.Description)].Should().Be(description is not null);
    }

    #endregion

    #endregion
}