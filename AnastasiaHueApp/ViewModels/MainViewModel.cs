using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger)
    : BaseViewModel
{
    [ObservableProperty] private string _text = "TestText!";
    [ObservableProperty] private string _boxText = string.Empty;

    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost/api/"), // NOTE: If using port 80 no port needs to be specified.
    };

    [RelayCommand]
    private async Task RetrieveBridgeConfig()
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync("", new
            {
                devicetype = "my_hue_app#iphone peter", // From: https://developers.meethue.com/develop/get-started-2/
            });
            response.EnsureSuccessStatusCode();
            var respString = await response.Content.ReadAsStringAsync();
            BoxText = respString;
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning null.", e.StatusCode);
        }
    }
}