using AnastasiaHueApp.Util.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Extensions;

[TestClass]
[TestSubject(typeof(EnumExtensions))]
public class EnumExtensionsTest
{
    #region TestClasses

    public enum TestEnum
    {
        Apple,
        MotorCycle,
        BateBorisov,
        Brick,
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        FULLCAPS,
        // ReSharper disable once InconsistentNaming
        lowercase,
    }

    #endregion


    #region GetName

    [TestMethod]
    [DataRow(TestEnum.Apple, "apple")]
    [DataRow(TestEnum.MotorCycle, "motorcycle")]
    [DataRow(TestEnum.BateBorisov, "bateborisov")]
    [DataRow(TestEnum.Brick, "brick")]
    [DataRow(TestEnum.FULLCAPS, "fullcaps")]
    [DataRow(TestEnum.lowercase, "lowercase")]
    public void GetName_ReturnsLowercaseName_IsTrue(TestEnum enumValue, string? correctString)
    {
        enumValue.GetName().Should().Be(correctString);
    }

    #endregion
}