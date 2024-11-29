using System;
using AnastasiaHueApp.Util;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace AnastasiaHueApp.Tests.Util;

[TestClass]
[TestSubject(typeof(Either<object, object>))]
public class EitherTest
{
    private class TestClass1;
    private class TestClass2;

    #region Constructor

    [TestMethod]
    public void Constructor_StringTypesEqual_ThrowsArgumentException()
    {
        var action = () => new Either<string, string>();
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_IntTypesEqual_ThrowsArgumentException()
    {
        var action = () => new Either<int, int>();
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_FloatTypesEqual_ThrowsArgumentException()
    {
        var action = () => new Either<float, float>();
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass1TypesEqual_ThrowsArgumentException()
    {
        var action = () => new Either<TestClass1, TestClass1>();
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass2TypesEqual_ThrowsArgumentException()
    {
        var action = () => new Either<TestClass2, TestClass2>();
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(int.MinValue)]
    [DataRow(int.MaxValue)]
    public void Constructor_StringIntTypesDoNotEqualAndOneIsNotNull_DoesNotThrowArgumentException(int x)
    {
        var action = () => new Either<string, int>(x);
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    [DataRow("")]
    public void Constructor_StringTestClass1TypesDoNotEqualAndOneIsNotNull_DoesNotThrowArgumentException(string x)
    {
        var action = () => new Either<string, TestClass1>(x);
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    [DataRow("")]
    public void Constructor_StringTestClass2TypesDoNotEqualAndOneIsNotNull_DoesNotThrowArgumentException(string x)
    {
        var action = () => new Either<string, TestClass2>(x);
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass1TestClass2TypesDoNotEqualAndOneIsNotNull_ThrowArgumentException()
    {
        var action = () => new Either<TestClass1, TestClass2>(new TestClass1());
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    [DataRow("", "")]
    public void Constructor_StringStringNeitherNull_ThrowsArgumentException(string value1, string value2)
    {
        var action = () => new Either<string, string>(value1, value2);
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass1TestClass2NeitherNull_ThrowsArgumentException()
    {
        var action = () => new Either<TestClass1, TestClass2>(new TestClass1(), new TestClass2());
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(null, null)]
    public void Constructor_StringAndIntBothNull_ThrowsArgumentException(string? str, int? n)
    {
        var action = () => new Either<string?, int?>(str, n);
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass1TestClass2BothNull_ThrowsArgumentException()
    {
        var action = () => new Either<TestClass1, TestClass2>(null, null);
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region IsType

    [TestMethod]
    public void IsType_TestClass1RetrievalFromEitherStringTestClass1IsTestClass1_ReturnsTrue()
    {
        // Arrange.
        var either = new Either<TestClass1, string>(new TestClass1());
    
        // Act.
        var isTestClass1 = either.IsType<TestClass1>(out var value);
    
        // Assert.
        isTestClass1.Should().BeTrue();
    }

    [TestMethod]
    public void IsType_TestClass2RetrievalFromEitherTestClass1TestClass2IsTestClass2_ReturnsTrue()
    {
        // Arrange.
        var either = new Either<TestClass1, TestClass2>(new TestClass2());

        // Act.
        var isTestClass2 = either.IsType<TestClass2>(out var value);

        // Assert.
        isTestClass2.Should().BeTrue();
    }

    [TestMethod]
    public void IsType_TestClass2RetrievalFromEitherTestClass1TestClass2IsTestClass1_ReturnsFalse()
    {
        // Arrange.
        var either = new Either<TestClass1, TestClass2>(new TestClass1());

        // Act.
        var isTestClass2 = either.IsType<TestClass2>(out var value);

        // Assert.
        isTestClass2.Should().BeFalse();
    }

    #endregion
}