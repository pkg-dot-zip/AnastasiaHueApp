using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util;
using AnastasiaHueApp.Util.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AnastasiaHueApp.ViewModels;

public abstract class BaseViewModel(IDisplayAlertHandler displayAlertHandler) : ObservableObject
{
    protected async Task ShowAlertOnError<T>(Either<T, ErrorResponse?> either)
    {
        if (either.IsType<ErrorResponse>(out var error)) await ShowAlertOnError(error);
    }

    protected async Task ShowAlertOnError(ErrorResponse? error)
    {
        if (error is not null) await displayAlertHandler.DisplayAlert(error);
    }
}