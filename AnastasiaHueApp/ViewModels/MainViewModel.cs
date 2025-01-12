﻿using System.Collections.ObjectModel;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Shell;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger,
    IDisplayAlertHandler displayAlertHandler,
    IHueHandler hueHandler,
    IShellContainer shellContainer)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : BaseViewModel(displayAlertHandler)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [ObservableProperty] private string _boxText = string.Empty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedColorBrush), nameof(SelectedColor))]
    private ObservableCollection<HueLight> _lights = [];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedColorBrush), nameof(SelectedColor))]
    private int _selectedLightIndex;

    [ObservableProperty] private bool _isRefreshing;

    public Brush SelectedColorBrush
    {
        get => new SolidColorBrush(SelectedColor);
        set => OnPropertyChanged();
    }

    public Microsoft.Maui.Graphics.Color SelectedColor
    {
        get
        {
            if (SelectedLightIndex == -1) return Color.FromRgb(0, 0, 0); // Avoids out of range exception when fiddling with page navigation.
            return Lights[SelectedLightIndex].State.Color.ToMauiColor();
        }
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

        if (either.IsType<UsernameResponse>(out var username))
        {
            BoxText = username!.Username;

            // If successful, immediately retrieve lights and navigate to lights page.
            await RefreshLights();
            await shellContainer.CurrentShell.GoToAsync("///LightsPage");
        }
        await ShowAlertOnError(either!);
    }

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
    private async Task MakeLightBlinkFor10Sec()
    {
        var light = Lights[SelectedLightIndex];
        await ShowAlertOnError(await hueHandler.MakeLightBlink(light.Id));
    }

    [RelayCommand]
    private async Task MakeLightColorLoop()
    {
        var light = Lights[SelectedLightIndex];
        await ShowAlertOnError(await hueHandler.MakeLightColorLoop(light.Id));
    }

    [RelayCommand]
    private async Task RefreshLights()
    {
        IsRefreshing = true;
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

        await Task.Delay(2000); // We load too fast, so we don't see the animation unless we wait.
        IsRefreshing = false;
    }
}