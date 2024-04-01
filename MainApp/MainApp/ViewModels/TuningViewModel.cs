using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? _plate;
    [ObservableProperty] private CarData? _carData;
    private readonly IApiService _apiService = new ApiService();

    [RelayCommand]
    private async Task Submit(string licensePlate)
    {
        await Shell.Current.GoToAsync($"TuningResultPage?license={licensePlate}");
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var licensePlate = Plate = query["license"] as string;

        // Create separate thread to prevent deadlock
        if (licensePlate != null)
        {
            Task.Run(async () =>
            {
                CarData = await _apiService.QueryRdwData(licensePlate);
            }).GetAwaiter().GetResult();
        }

        Trace.WriteLine(licensePlate);
    }
}