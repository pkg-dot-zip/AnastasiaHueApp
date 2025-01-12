﻿using AnastasiaHueApp.Models.Message;

namespace AnastasiaHueApp.Util.Alerts;

/// <summary>
/// Simple class that handles alert, implementing an interface so we can mock it and our code is not UI dependent.
/// </summary>
public class DisplayAlertHandler : IDisplayAlertHandler
{
    /// <inheritdoc cref="IDisplayAlertHandler.DisplayAlert(string?, string, string)"/>
    public async Task<bool> DisplayAlert(string? title, string message, string cancel = "OK")
    {
        return await DisplayAlert(title, message, null, cancel, FlowDirection.MatchParent);
    }

    /// <inheritdoc cref="IDisplayAlertHandler.DisplayAlert(string?, string, string?, string, FlowDirection)"/>
    public async Task<bool> DisplayAlert(string? title, string message, string? accept, string cancel, FlowDirection flowDirection)
    {
        // TODO: Fix deprecated issue. NOTE: Won't change this since this is just a simple school assignment and not a long term project.
        return await Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel, flowDirection);
    }

    /// <inheritdoc cref="IDisplayAlertHandler.DisplayAlert(ErrorResponse)"/>
    public async Task<bool> DisplayAlert(ErrorResponse error)
    {
        return await DisplayAlert($"Error {error.Type}", error.Description);
    }
}