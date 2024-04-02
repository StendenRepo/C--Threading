using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;
using MainApp.Models;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? _plate;
    [ObservableProperty] private CarData? _carData;
    private readonly IApiService _apiService = new ApiService();
    private readonly CarSpecsWebScraper _webScraper = new();
    
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
                await FetchCarData(licensePlate);
            }).GetAwaiter().GetResult();
        }

        Trace.WriteLine(licensePlate);
    }

    private async Task FetchCarData(string licensePlate)
    {
        CarData = await _apiService.QueryRdwData(licensePlate);
        CarData.ScrapedCarData = _webScraper.GetCarSpecs(licensePlate);
    }
}