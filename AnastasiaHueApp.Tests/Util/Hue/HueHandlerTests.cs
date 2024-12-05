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
}
