using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Color;

[TestClass]
[TestSubject(typeof(AnastasiaHueApp.Util.Color.Color))]
public class ColorTest
{
    #region FromHueHsb

    #region FromHueHsbArguments

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

    #endregion

    #region FromRgb

    #region FromRgbArguments

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

    #region FromRgbValue

    [TestMethod]
    [DataRow(new int[] { 255, 255, 255 }, new int[] {0, 0, 254})] // White.
    [DataRow(new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 })] // Black.
    [DataRow(new int[] { 255, 0, 0 }, new int[] { 0, 254, 254 })] // Red.
    [DataRow(new int[] { 0, 255, 0 }, new int[] { 21845, 254, 254 })] // Green.
    [DataRow(new int[] { 0, 0, 255 }, new int[] { 43690, 254, 254 })] // Blue.
    public void FromRgb_ColorEqualsHsbColor_ReturnsTrue(int[] rgb, int[] hsb)
    {
        // Arrange.
        var shouldBeColor = AnastasiaHueApp.Util.Color.Color.FromHueHsb(hsb[0], hsb[1], hsb[2]);
        var rgbColor = AnastasiaHueApp.Util.Color.Color.FromRgb(rgb[0], rgb[1], rgb[2]);

        // Act & Assert.
        shouldBeColor.Equals(rgbColor).Should().BeTrue($"because {rgbColor} does not equal {shouldBeColor}");
    }

    #endregion

    #endregion

    #region FromHex

    #region FromHexValue

    [TestMethod]
    [DataRow(0xFFFFFFFF, new int[]{255, 255, 255})] // Black.
    [DataRow(0xFF000000, new int[]{0, 0, 0})] // White.
    [DataRow(0xFFFF0000, new int[]{255, 0, 0})] // Red.
    [DataRow(0xFF00FF00, new int[]{0, 255, 0})] // Green.
    [DataRow(0xFF0000FF, new int[]{0, 0, 255})] // Blue.
    // NOTE: Colors picked from the embedded color picker on the Google search engine.
    [DataRow(0xFF32a852, new int[]{50, 168, 82})] // A type of green.
    [DataRow(0xFF1d4fdb, new int[]{29, 79, 219})] // Ikea-ish blue.
    [DataRow(0xFFe1ff00, new int[]{255, 255, 0})] // Yellow.
    public void FromHex_ColorEqualsRgbColor_ReturnsTrue(uint hex, int[] rgb)
    {
        // Arrange.
        var shouldBeColor = AnastasiaHueApp.Util.Color.Color.FromHex(hex);
        var rgbColor = AnastasiaHueApp.Util.Color.Color.FromRgb(rgb[0], rgb[1], rgb[2]);

        // Act & Assert.
        shouldBeColor.Equals(rgbColor).Should().BeTrue($"because {rgbColor} does not equal {shouldBeColor}");
    }

    #endregion

    #endregion

    #region Equals

    [TestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(10, 10, 10)]
    [DataRow(0, 254, 254)]
    [DataRow(65535, 254, 254)]
    public void Equals_Color1EqualsColor2_ReturnsTrue(int h, int s, int b)
    {
        // Arrange.
        var color1 = AnastasiaHueApp.Util.Color.Color.FromHueHsb(h, s, b);
        var color2 = AnastasiaHueApp.Util.Color.Color.FromHueHsb(h, s, b);

        // Act & Assert.
        color1.Equals(color2).Should().BeTrue();
    }

    [TestMethod]
    [DataRow(new int[]{0, 0, 0}, new int[]{10, 20, 30})]
    [DataRow(new int[] { 10, 10, 10 }, new int[] { 10, 20, 30 })]
    [DataRow(new int[] { 0, 254, 254 }, new int[] { 0, 0, 254 })]
    [DataRow(new int[] { 0, 254, 254 }, new int[] { 1, 253, 253 })]
    public void Equals_Color1DoesNotEqualColor2_ReturnsFalse(int[] hsb1, int[] hsb2)
    {
        // Arrange.
        var color1 = AnastasiaHueApp.Util.Color.Color.FromHueHsb(hsb1[0], hsb1[1], hsb1[2]);
        var color2 = AnastasiaHueApp.Util.Color.Color.FromHueHsb(hsb2[0], hsb2[1], hsb2[2]);

        // Act & Assert.
        color1.Equals(color2).Should().BeFalse();
    }

    #endregion
}