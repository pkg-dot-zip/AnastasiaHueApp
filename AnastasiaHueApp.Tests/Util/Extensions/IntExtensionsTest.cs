using AnastasiaHueApp.Util.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Extensions;

[TestClass]
[TestSubject(typeof(IntExtensions))]
public class IntExtensionsTest
{
    #region IsInRange

    [TestMethod]
    [DataRow(0, int.MinValue, int.MaxValue)] // N zero.
    [DataRow(2, 1, 3)] // All positive
    [DataRow(1, 1, 1)] // All equal.
    [DataRow(-1, -2, 0)] // N negative.
    [DataRow(-2, -3, -1)] // All negative.
    public void IsInRange_NBetweenAAndB_ReturnsTrue(int n, int a, int b)
    {
        n.IsInRange(a, b).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(int.MinValue, 0, int.MaxValue)] // Far off.
    [DataRow(0, 1, 2)] // 1 off, all positive.
    [DataRow(-1, 0, 1)] // N negative.
    [DataRow(-10, -9, -8)] // 1 off, all negative.
    public void IsInRange_NNotBetweenAAndB_ReturnsFalse(int n, int a, int b)
    {
        n.IsInRange(a, b).Should().BeFalse();
    }

    #endregion

}