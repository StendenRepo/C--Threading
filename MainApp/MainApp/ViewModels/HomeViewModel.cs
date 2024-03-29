using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MainApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _plate;
    
    [RelayCommand]
    private async Task Submit()
    {
        await Shell.Current.GoToAsync($"TuningResultPage?license={Plate}");
    }
}