using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.Util.Preferences;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;

namespace AnastasiaHueApp.Tests.Util.Hue;

[TestClass]
[TestSubject(typeof(HueHandler))]
public class HueHandlerTests
{
    #region IsOldConnectionValid

    [TestMethod]
    public async Task IsOldConnectionValid_UsernameFound_HttpRequestException_ReturnsFalse()
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();

        //      PreferencesHandler.
        var storageHandler = new Mock<IStorageHandler>();
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);


        //      HttpClient.
        var clientContainer = new Mock<IHttpClientContainer>();
        var httpHandler = new MockHttpMessageHandler();
        httpHandler.When("*").Throw(new HttpRequestException());

        clientContainer
            .Setup(c => c.HttpClient)
            .Returns(httpHandler.ToHttpClient);

        //      Finish arranging.
        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var (isValid, _) = await hueHandler.IsOldConnectionValid();

        // Assert.
        isValid.Should().BeFalse();
    }

    #endregion

    #region GetLight

    [TestMethod]
    [DataRow(0)] // Zero.
    [DataRow(-1)] // One less than zero.
    [DataRow(int.MinValue)] // Extreme.
    public async Task GetLight_LightIdEqualOrLessThan0_ThrowsArgumentOutOfRangeException(int lightId)
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();
        var storageHandler = new Mock<IStorageHandler>();
        var clientContainer = new Mock<IHttpClientContainer>();

        //      StorageHandler needs to return a username, see the IsAllowedToMakeCall() private method in HueHandler.
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);

        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var action = async () => await hueHandler.GetLight(lightId);

        // Assert.
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    #endregion

    #region LightSwitch

    [TestMethod]
    [DataRow(0)] // Zero.
    [DataRow(-1)] // One less than zero.
    [DataRow(int.MinValue)] // Extreme.
    public async Task LightSwitch_LightIdEqualOrLessThan0_ThrowsArgumentOutOfRangeException(int lightId)
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();
        var storageHandler = new Mock<IStorageHandler>();
        var clientContainer = new Mock<IHttpClientContainer>();

        //      StorageHandler needs to return a username, see the IsAllowedToMakeCall() private method in HueHandler.
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);

        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var action = async () => await hueHandler.LightSwitch(lightId, true);

        // Assert.
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    #endregion

    #region SetColorTo

    [TestMethod]
    [DataRow(0)] // Zero.
    [DataRow(-1)] // One less than zero.
    [DataRow(int.MinValue)] // Extreme.
    public async Task SetColorTo_LightIdEqualOrLessThan0_ThrowsArgumentOutOfRangeException(int lightId)
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();
        var storageHandler = new Mock<IStorageHandler>();
        var clientContainer = new Mock<IHttpClientContainer>();

        //      StorageHandler needs to return a username, see the IsAllowedToMakeCall() private method in HueHandler.
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);

        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var action = async () => await hueHandler.SetColorTo(lightId, AnastasiaHueApp.Util.Color.Color.FromRgb(0, 0, 0));

        // Assert.
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    #endregion

    #region MakeLightBlink

    [TestMethod]
    [DataRow(0)] // Zero.
    [DataRow(-1)] // One less than zero.
    [DataRow(int.MinValue)] // Extreme.
    public async Task MakeLightBlink_LightIdEqualOrLessThan0_ThrowsArgumentOutOfRangeException(int lightId)
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();
        var storageHandler = new Mock<IStorageHandler>();
        var clientContainer = new Mock<IHttpClientContainer>();

        //      StorageHandler needs to return a username, see the IsAllowedToMakeCall() private method in HueHandler.
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);

        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var action = async () => await hueHandler.MakeLightBlink(lightId);

        // Assert.
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    #endregion

    #region MakeLightColorLoop

    [TestMethod]
    [DataRow(0)] // Zero.
    [DataRow(-1)] // One less than zero.
    [DataRow(int.MinValue)] // Extreme.
    public async Task MakeLightColorLoop_LightIdEqualOrLessThan0_ThrowsArgumentOutOfRangeException(int lightId)
    {
        // Arrange.
        var logger = new Mock<ILogger<HueHandler>>();
        var jsonRegistry = new Mock<IJsonRegistry>();
        var storageHandler = new Mock<IStorageHandler>();
        var clientContainer = new Mock<IHttpClientContainer>();

        //      StorageHandler needs to return a username, see the IsAllowedToMakeCall() private method in HueHandler.
        const string beautifulUsername = "64147585bf0468ca570336b8703aa50";
        storageHandler.Setup(h => h.RetrieveUsername()).Returns(beautifulUsername);

        HueHandler hueHandler = new HueHandler(logger.Object, jsonRegistry.Object, storageHandler.Object,
            clientContainer.Object);

        // Act.
        var action = async () => await hueHandler.MakeLightColorLoop(lightId);

        // Assert.
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    #endregion
}
