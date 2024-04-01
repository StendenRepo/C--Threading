using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject
{
    [ObservableProperty] private string? _plate;
    [ObservableProperty] private CarData? _carData;
    private readonly IApiService _apiService = new ApiService();

    [RelayCommand]
    private async Task SetCarData()
    {
        var test = Plate;
        CarData = await this._apiService.QueryRdwData(Plate);
    }

    [RelayCommand]
    private async Task Submit()
    {
        await Shell.Current.GoToAsync($"TuningResultPage");
        await SetCarData();
    }
    
    // public void ApplyQueryAttributes(IDictionary<string, object> query)
    // {
    //     var licensePlate = query["license"] as string;
    //     Plate = query["license"] as string;
    //
    //     if (licensePlate != null)
    //     {
    //         Task.Run(async () =>
    //         {
    //             await SetCarData(licensePlate);
    //         }).GetAwaiter().GetResult();
    //     }
    //
    //     Trace.WriteLine(licensePlate);
    // }
}