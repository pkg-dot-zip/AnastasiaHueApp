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

    [RelayCommand]
    private async Task RetrieveBridgeConfig()
    {
        try
        {
            var either = await hueHandler.AttemptLinkAsync();

            if (either.IsType<UsernameResponse>(out var username))
            {
                BoxText = username!.Username;
            }

            if (either.IsType<ErrorResponse>(out var error))
            {
                await displayAlertHandler.DisplayAlert("Error", error!.Description);
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning null.", e.StatusCode);
            await displayAlertHandler.DisplayAlert("NETWORK ERROR", "Code {0}. Returning null.");
        }
    }
}