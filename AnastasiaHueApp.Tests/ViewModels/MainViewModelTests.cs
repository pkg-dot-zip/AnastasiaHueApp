using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Shell;
using AnastasiaHueApp.ViewModels;
using Castle.Core.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Color = AnastasiaHueApp.Util.Color.Color;

namespace AnastasiaHueApp.Tests.ViewModels;

[TestClass]
[TestSubject(typeof(MainViewModel))]
public class MainViewModelTests
{
    #region BridgeCommand

    [TestMethod]
    public void BridgeCommand_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.AttemptLinkAsync())
            .ReturnsAsync(() => new Either<UsernameResponse, ErrorResponse>(error));

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object);

        // Act.
        viewModel.RetrieveBridgeConfigCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion


    #region SwitchLight

    [TestMethod]
    public void SwitchLight_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.LightSwitch(1, true))
            .ReturnsAsync(error);

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object)
            {
                SelectedLightIndex = 0,
                Lights = [new HueLight
                {
                    Id = 1,
                    State = new HueLightState
                    {
                        On = true,
                    }
                }]
            };

        // Act.
        viewModel.SwitchLightCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion

    #region ChangeLightColor

    [TestMethod]
    public void ChangeLightColor_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var color = Color.FromRgb(0, 0, 0);

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.SetColorTo(1, color))
            .ReturnsAsync(error);

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object)
        {
            SelectedLightIndex = 0,
            Lights = [new HueLight
            {
                Id = 1,
                State = new HueLightState
                {
                    On = true,
                    Hue = 0,
                    Saturation = 0,
                    Brightness = 0,
                }
            }]
        };

        // Act.
        viewModel.ChangeLightColorCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion

    #region MakeLightBlinkFor10Sec

    [TestMethod]
    public void MakeLightBlinkFor10Sec_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.MakeLightBlink(1))
            .ReturnsAsync(error);

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object)
        {
            SelectedLightIndex = 0,
            Lights = [new HueLight
            {
                Id = 1,
                State = new HueLightState
                {
                   Alert = HueAlert.None,
                }
            }]
        };

        // Act.
        viewModel.MakeLightBlinkFor10SecCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion

    #region MakeLightColorLoop

    [TestMethod]
    public void MakeLightColorLoop_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.MakeLightColorLoop(1))
            .ReturnsAsync(error);

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object)
        {
            SelectedLightIndex = 0,
            Lights = [new HueLight
            {
                Id = 1,
                State = new HueLightState
                {
                    Effect = HueEffect.None,
                }
            }]
        };

        // Act.
        viewModel.MakeLightColorLoopCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion

    #region RefreshLights

    [TestMethod]
    public void RefreshLights_HueHandlerReturnsErrorResponse_ShowsDisplayAlert()
    {
        // Arrange.
        var error = new ErrorResponse
        {
            Address = string.Empty,
            Description = string.Empty,
            Type = string.Empty,
        };

        var logger = new Mock<ILogger<MainViewModel>>();
        var shellContainer = new Mock<IShellContainer>();

        //      DisplayAlertHandlerMock.
        var alertHandler = new Mock<IDisplayAlertHandler>();
        alertHandler.Setup(h => h.DisplayAlert(error));

        //      HueHandler Mock.
        var hueHandler = new Mock<IHueHandler>();
        hueHandler
            .Setup(h => h.GetLights())
            .ReturnsAsync(new Either<List<HueLight>, ErrorResponse>(error));

        var viewModel = new MainViewModel(logger.Object, alertHandler.Object, hueHandler.Object, shellContainer.Object);

        // Act.
        viewModel.RefreshLightsCommand.Execute(null);

        // Assert.
        alertHandler.Verify(mock => mock.DisplayAlert(error), Times.AtLeastOnce);
    }

    #endregion
}