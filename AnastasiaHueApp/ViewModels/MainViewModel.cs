using System.Collections.ObjectModel;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Color;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Hue;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger,
    IDisplayAlertHandler displayAlertHandler,
    IHueHandler hueHandler)
    : BaseViewModel
{
    [ObservableProperty] private string _boxText = string.Empty;
    [ObservableProperty] private int _lightSelectedValueStepper = 1;

    [ObservableProperty] private ObservableCollection<HueLight> _lights = new();
    [ObservableProperty] private int _selectedLightIndex;

    [RelayCommand]
    private async Task RetrieveBridgeConfig()
    {
        var either = await hueHandler.AttemptLinkAsync();

        if (either.IsType<UsernameResponse>(out var username)) BoxText = username!.Username;
        await ShowAlertOnError(either!);
    }

    [RelayCommand]
    private async Task RetrieveAllLights()
    {
        var either = await hueHandler.GetLights();

        if (either.IsType<List<HueLight>>(out var lights))
        {
            foreach (var light in lights!)
            {
                logger.LogInformation($"({light.Id}) | {light.Name} - {light.State.Brightness}");
            }

            Lights.Clear();
            Lights.AddAll(lights);
        }

        await ShowAlertOnError(either!);
    }

    [RelayCommand]
    private async Task RetrieveLight()
    {
        var either = await hueHandler.GetLight(LightSelectedValueStepper);

        if (either.IsType<HueLight>(out var light))
        {
            logger.LogInformation($"({light!.Id}) | {light.Name} - {light.State.Brightness}");
        }

        await ShowAlertOnError(either!);
    }

    [RelayCommand]
    private async Task TurnLightOn() => await ShowAlertOnError(await hueHandler.LightSwitch(LightSelectedValueStepper, true));

    [RelayCommand]
    private async Task TurnLightOff() => await ShowAlertOnError(await hueHandler.LightSwitch(LightSelectedValueStepper, false));

    [RelayCommand]
    private async Task SetLightToColor(RgbColor rgb) =>
        await ShowAlertOnError(await hueHandler.SetColorTo(LightSelectedValueStepper, rgb.ToColor()));

    [RelayCommand]
    private async Task MakeLightBlinkFor10Sec() => await ShowAlertOnError(await hueHandler.MakeLightBlink(LightSelectedValueStepper));

    [RelayCommand]
    private async Task MakeLightColorLoop() => await ShowAlertOnError(await hueHandler.MakeLightColorLoop(LightSelectedValueStepper));

    private async Task ShowAlertOnError<T>(Either<T, ErrorResponse?> either)
    {
        if (either.IsType<ErrorResponse>(out var error)) await ShowAlertOnError(error); 
    }

    private async Task ShowAlertOnError(ErrorResponse? error)
    {
        if (error is not null) await displayAlertHandler.DisplayAlert(error);
    }

    // [RelayCommand]
    // private async Task SetLightState()
    // {
    //     var response = await hueHandler.SetLightState(Lights[SelectedLightIndex].Id, Lights[SelectedLightIndex].State);
    //     await ShowAlertOnError(response);
    // }

    [RelayCommand]
    private async Task SwitchLight()
    {
        var selectedLight = Lights[SelectedLightIndex];
        await ShowAlertOnError(await hueHandler.LightSwitch(selectedLight.Id,
            (bool)selectedLight.State.On!));
    }
}