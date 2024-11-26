using System;
using AnastasiaHueApp.Util;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public void Constructor_StringIntTypesDoNotEqual_DoesNotThrowArgumentException()
    {
        var action = () => new Either<string, int>();
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_StringTestClass1TypesDoNotEqual_DoesNotThrowArgumentException()
    {
        var action = () => new Either<string, TestClass1>();
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_StringTestClass2TypesDoNotEqual_DoesNotThrowArgumentException()
    {
        var action = () => new Either<string, TestClass2>();
        action.Should().NotThrow<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_TestClass1TestClass2TypesDoNotEqual_DoesNotThrowArgumentException()
    {
        var action = () => new Either<TestClass1, TestClass2>();
        action.Should().NotThrow<ArgumentException>();
    }

    #endregion

    #region IsType

    [TestMethod]
    public void IsType_TestClass1RetrievalFromEitherStringTestClass1_ReturnsTrue()
    {
        // Arrange.
        var either = new Either<TestClass1, string>(new TestClass1());
    
        // Act.
        var isTestClass1 = either.IsType<TestClass1>(out var value);
    
        // Assert.
        isTestClass1.Should().BeTrue();
    }

    #endregion
}