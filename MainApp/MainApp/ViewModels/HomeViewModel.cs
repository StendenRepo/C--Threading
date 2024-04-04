using System.Diagnostics;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;
using MainApp.Models;

namespace MainApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _plate;
    private readonly IApiService _service = new ApiService();
    
    [RelayCommand]
    private async Task Submit()
    {
        await Shell.Current.GoToAsync($"TuningResultPage?license={Plate}");
    }

}