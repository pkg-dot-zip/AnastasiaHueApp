using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger,
    IDisplayAlertHandler displayAlertHandler,
    HueHandler hueHandler)
    : BaseViewModel
{
    [ObservableProperty] private string _text = "TestText!";
    [ObservableProperty] private string _boxText = string.Empty;
    [ObservableProperty] private int _lightSelectedValueStepper = 1;

    private string _username = string.Empty;

    [RelayCommand]
    private async Task RetrieveBridgeConfig()
    {
        var either = await hueHandler.AttemptLinkAsync();

        if (either.IsType<UsernameResponse>(out var username))
        {
            BoxText = username!.Username;
            _username = username.Username;
        }

        if (either.IsType<ErrorResponse>(out var error)) await displayAlertHandler.DisplayAlert(error!);
    }

    [RelayCommand]
    private async Task RetrieveAllLights()
    {
        var either = await hueHandler.GetLights(_username);

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
        var either = await hueHandler.GetLight(_username, LightSelectedValueStepper);

        if (either.IsType<HueLight>(out var light))
        {
            logger.LogInformation($"{light!.Name} - {light.State.Brightness}");
        }

        if (either.IsType<ErrorResponse>(out var error)) await displayAlertHandler.DisplayAlert(error!);
    }

    [RelayCommand]
    private async Task TurnLightOn() => await hueHandler.LightSwitch(_username, LightSelectedValueStepper, true);

    [RelayCommand]
    private async Task TurnLightOff() => await hueHandler.LightSwitch(_username, LightSelectedValueStepper, false);
}