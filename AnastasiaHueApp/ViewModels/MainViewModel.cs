using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.ViewModels;

public partial class MainViewModel(
    ILogger<MainViewModel> logger)
    : BaseViewModel
{
    [ObservableProperty]
    private string _text = "TestText!";
}