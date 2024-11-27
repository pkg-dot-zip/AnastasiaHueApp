using System.Text.Json;
using AnastasiaHueApp.Util.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnastasiaHueApp.Tests.Util.Extensions;

[TestClass]
[TestSubject(typeof(JsonElementExtensions))]
public class JsonElementExtensionsTest
{
    private enum TestEnum1
    {
        Value1,
        Value2,
        Value3,
    }

    [TestMethod] // TODO: Add difference in casing.
    public void GetEnum_CanParseTestEnum1Values_DoesNotThrow()
    {
        var doc = """
                  {
                    "testName": "Value2"
                  }
                  """;
        var action = () => JsonDocument.Parse(doc).RootElement.GetProperty("testName").GetEnum<TestEnum1>();
        action.Should().NotThrow();
    }

    [TestMethod] // TODO: Add difference in casing.
    public void GetEnum_ParsesCorrectValueTestEnum1_AssertsTrue()
    {
        var doc = """
                  {
                    "testName": "Value2"
                  }
                  """;
        var enumValue = JsonDocument.Parse(doc).RootElement.GetProperty("testName").GetEnum<TestEnum1>();
        enumValue.Should().Be(TestEnum1.Value2);
    }
}