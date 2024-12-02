using AnastasiaHueApp.Models;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Color;
using AnastasiaHueApp.Util.Hue;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class DevViewModel(ILogger<DevViewModel> logger, IHueHandler hueHandler, IDisplayAlertHandler displayAlertHandler) : BaseViewModel(displayAlertHandler)
{
    [ObservableProperty] private int _lightSelectedValueStepper = 1;

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
    private async Task TurnLightOn() =>
        await ShowAlertOnError(await hueHandler.LightSwitch(LightSelectedValueStepper, true));

    [RelayCommand]
    private async Task TurnLightOff() =>
        await ShowAlertOnError(await hueHandler.LightSwitch(LightSelectedValueStepper, false));

    [RelayCommand]
    private async Task SetLightToColor(RgbColor rgb) =>
        await ShowAlertOnError(await hueHandler.SetColorTo(LightSelectedValueStepper, rgb.ToColor()));

    [RelayCommand]
    private async Task MakeLightBlinkFor10Sec() =>
        await ShowAlertOnError(await hueHandler.MakeLightBlink(LightSelectedValueStepper));

    [RelayCommand]
    private async Task MakeLightColorLoop() =>
        await ShowAlertOnError(await hueHandler.MakeLightColorLoop(LightSelectedValueStepper));
}