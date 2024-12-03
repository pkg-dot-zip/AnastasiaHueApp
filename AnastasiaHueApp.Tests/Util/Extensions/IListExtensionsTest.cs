using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnastasiaHueApp.Util.Extensions;
using FluentAssertions;
using JetBrains.Annotations;

namespace AnastasiaHueApp.Tests.Util.Extensions;

[TestClass]
[TestSubject(typeof(IListExtensions))]
// ReSharper disable once InconsistentNaming
public class IListExtensionsTests
{
    #region AddAll

    [TestMethod]
    [DataRow(new int[] { 1, 2, 3 })]
    [DataRow(new int[] { 8, 2, 3 })]
    [DataRow(new int[] { -2398, 21985, 12598 })]
    [DataRow(new int[] { -298, 215, -198 })]
    public void AddAllEnumerable_IntData_Contains_IntData_ReturnTrue(int[] value)
    {
        // Arrange.
        var list = new List<int>();
        var listToAddFrom = value.ToList();

        // Act.
        // ReSharper disable once PossibleMultipleEnumeration
        list.AddAll(listToAddFrom);

        // Assert.
        list.Count.Should().NotBe(0);
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var i in listToAddFrom)
        {
            list.Contains(i).Should().BeTrue();
        }
    }

    [TestMethod]
    [DataRow(new int[] { 1, 2, 3 })]
    [DataRow(new int[] { 8, 2, 3 })]
    [DataRow(new int[] { -2398, 21985, 12598 })]
    [DataRow(new int[] { -298, 215, -198 })]
    public void AddAllParams_IntData_Contains_IntData_ReturnTrue(int[] value)
    {
        // Arrange.
        var list = new List<int>();

        // Act.
        // ReSharper disable once PossibleMultipleEnumeration
        list.AddAll(value);

        // Assert.
        list.Count.Should().NotBe(0);
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var i in value)
        {
            list.Contains(i).Should().BeTrue();
        }
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(123)]
    [DataRow(-1241)]
    [DataRow(int.MaxValue)]
    [DataRow(int.MinValue)]
    public void AddAllEnumerable_Int_EndsWith_Int_ReturnTrue(int value)
    {
        // Arrange.
        var list = new List<int> { 98, 912, 173, 123 };
        var listToAddFrom = new List<int> { value };
        
        // Act.
        list.AddAll(listToAddFrom);

        // Assert.
        listToAddFrom.Last().Should().Be(value);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(123)]
    [DataRow(-1241)]
    [DataRow(int.MaxValue)]
    [DataRow(int.MinValue)]
    public void AddAllParams_Int_EndsWith_Int_ReturnTrue(int value)
    {
        // Arrange.
        var list = new List<int> { 98, 912, 173, 123 };
        var listToAddFrom = new[] { value };

        // Act.
        list.AddAll(listToAddFrom);

        // Assert.
        listToAddFrom.Last().Should().Be(value);
    }

    #endregion
}