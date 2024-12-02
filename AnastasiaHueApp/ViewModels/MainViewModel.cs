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

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedColorBrush), nameof(SelectedColor))]
    private ObservableCollection<HueLight> _lights = [];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedColorBrush), nameof(SelectedColor))]
    private int _selectedLightIndex;

    public Brush SelectedColorBrush
    {
        get => new SolidColorBrush(SelectedColor);
        set => OnPropertyChanged();
    }

    public Microsoft.Maui.Graphics.Color SelectedColor
    {
        get => Lights[SelectedLightIndex].State.Color.ToMauiColor();
        set
        {
            OnPropertyChanged(nameof(SelectedColorBrush));
            OnPropertyChanged();
        }
    }

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
        var light = Lights[SelectedLightIndex];
        await ShowAlertOnError(await hueHandler.LightSwitch(light.Id,
            (bool)light.State.On!));
    }

    [RelayCommand]
    private async Task ChangeLightColor()
    {
        var light = Lights[SelectedLightIndex];
        SelectedColor = light.State.Color.ToMauiColor();
        await ShowAlertOnError(await hueHandler.SetColorTo(light.Id, light.State.Color));
    }

    [RelayCommand]
    private async Task RefreshLights()
    {
        logger.LogInformation("Attempting refresh.");

        // First we try to collect the new lights.
        var either = await hueHandler.GetLights();

        // Then, if successful, we remove existing lights, add the new ones and set the selectedIndex to 0.
        if (either.IsType<List<HueLight>>(out var lights))
        {
            logger.LogInformation("Successful refresh.");
            Lights.Clear();
            Lights.AddAll(lights!);
            SelectedLightIndex = 0;
        }

        // If not successful, we show the error why, then display an error that we failed to refresh. Meaning two alerts will be displayed in succession.
        if (either.IsType<ErrorResponse>(out var error))
        {
            logger.LogInformation("Failed refresh.");
            await ShowAlertOnError(error);
            await displayAlertHandler.DisplayAlert("Failed to refresh lights", "Try again later!");
        }
    }
}