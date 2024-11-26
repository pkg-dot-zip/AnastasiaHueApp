using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Color;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Hue;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Color = AnastasiaHueApp.Util.Color.Color;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger,
    IDisplayAlertHandler displayAlertHandler,
    IHueHandler hueHandler)
    : BaseViewModel
{
    [ObservableProperty] private string _boxText = string.Empty;
    [ObservableProperty] private int _lightSelectedValueStepper = 1;


    [RelayCommand]
    private async Task RetrieveBridgeConfig()
    {
        var either = await hueHandler.AttemptLinkAsync();

        if (either.IsType<UsernameResponse>(out var username)) BoxText = username!.Username;
        if (either.IsType<ErrorResponse>(out var error)) await displayAlertHandler.DisplayAlert(error!);
    }

    [RelayCommand]
    private async Task RetrieveAllLights()
    {
        var either = await hueHandler.GetLights();

        if (either.IsType<List<HueLight>>(out var lights))
        {
            foreach (var light in lights!)
            {
                logger.LogInformation($"{light.Name} - {light.State.Brightness}");
            }
        }

        if (either.IsType<ErrorResponse>(out var error)) await displayAlertHandler.DisplayAlert(error!);
    }

    [RelayCommand]
    private async Task RetrieveLight()
    {
        var either = await hueHandler.GetLight(LightSelectedValueStepper);

        if (either.IsType<HueLight>(out var light))
        {
            logger.LogInformation($"{light!.Name} - {light.State.Brightness}");
        }

        if (either.IsType<ErrorResponse>(out var error)) await displayAlertHandler.DisplayAlert(error!);
    }

    [RelayCommand]
    private async Task TurnLightOn() => await hueHandler.LightSwitch(LightSelectedValueStepper, true);

    [RelayCommand]
    private async Task TurnLightOff() => await hueHandler.LightSwitch(LightSelectedValueStepper, false);

    [RelayCommand]
    private async Task SetLightToRed() => await hueHandler.SetColorTo(LightSelectedValueStepper, ColorHandler.Red);

    [RelayCommand]
    private async Task SetLightToGreen() => await hueHandler.SetColorTo(LightSelectedValueStepper, ColorHandler.Green);

    [RelayCommand]
    private async Task SetLightToBlue() => await hueHandler.SetColorTo(LightSelectedValueStepper, ColorHandler.Blue);
}