using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Color;

[TestClass]
[TestSubject(typeof(AnastasiaHueApp.Util.Color.Color))]
public class ColorTest
{
    #region FromHueHsb

    [TestMethod]
    [DataRow(65536, 0, 0)] // Hue out of range.
    [DataRow(0, 255, 0)] // Saturation out of range. 
    [DataRow(0, 0, 255)] // Brightness out of range. 
    [DataRow(-1, -1, -1)] // All negative. 
    [DataRow(int.MaxValue, int.MaxValue, int.MaxValue)] // Extremes positive.
    [DataRow(int.MinValue, int.MinValue, int.MinValue)] // Extremes negative.
    public void FromHueHsb_ArgumentsOutOfRange_ThrowsArgumentException(int h, int s, int b)
    {
        var action = () => AnastasiaHueApp.Util.Color.Color.FromHueHsb(h, s, b);
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(65535, 254, 254)] // Upperbounds.
    [DataRow(0, 0, 0)] // Lower-bounds.
    [DataRow(100, 100, 100)] // In-between.
    public void FromHueHsb_ArgumentsInRange_DoesNotThrowArgumentException(int h, int s, int b)
    {
        var action = () => AnastasiaHueApp.Util.Color.Color.FromHueHsb(h, s, b);
        action.Should().NotThrow<ArgumentException>();
    }

    #endregion

    #region FromRgb

    [TestMethod]
    [DataRow(256, 0, 0)] // Red out of range.
    [DataRow(0, 256, 0)] // Green out of range. 
    [DataRow(0, 0, 256)] // Blue out of range. 
    [DataRow(-1, -1, -1)] // All negative. 
    [DataRow(int.MaxValue, int.MaxValue, int.MaxValue)] // Extremes positive.
    [DataRow(int.MinValue, int.MinValue, int.MinValue)] // Extremes negative.
    public void FromRgb_ArgumentsOutOfRange_ThrowsArgumentException(int r, int g, int b)
    {
        var action = () => AnastasiaHueApp.Util.Color.Color.FromRgb(r, g, b);
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(255, 255, 255)] // Upperbounds.
    [DataRow(0, 0, 0)] // Lower-bounds.
    [DataRow(122, 122, 122)] // In-between.
    public void FromRgb_ArgumentsInRange_DoesNotThrowArgumentException(int r, int g, int b)
    {
        var action = () => AnastasiaHueApp.Util.Color.Color.FromRgb(r, g, b);
        action.Should().NotThrow<ArgumentException>();
    }

    #endregion
}