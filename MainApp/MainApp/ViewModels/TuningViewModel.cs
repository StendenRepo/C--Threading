using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;
using MainApp.Models;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? _plate;
    [ObservableProperty] private CarData? _carData;
    [ObservableProperty] private TuningResult? _tuningResult;
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
            Task.Run(async () => { await FetchCarData(licensePlate); }).GetAwaiter().GetResult();
        }

        Trace.WriteLine(licensePlate);
    }

    private async Task FetchCarData(string licensePlate)
    {
        CarData = await _apiService.QueryRdwData(licensePlate);

        CarData.ScrapedCarData = _webScraper.GetCarSpecs(licensePlate);

        if (CarData.ScrapedCarData.HorsePower == null || CarData.ScrapedCarData.Torque == null) return;
        TuningResult = new TuningResult(CarData.ScrapedCarData.HorsePower, CarData.ScrapedCarData.Torque);
    }

    [RelayCommand]
    private async Task ExportData()
    {
        await FetchAllCarData();
    }

    private async Task FetchAllCarData()
    {
        var data = await _apiService.QueryAllRdwData();
        await SaveCarDataToFileAsync(data);
    }

    private Task SaveCarDataToFileAsync(IEnumerable<CarData> carData)
    {
        var saveThread = new Thread(async () =>
        {
            using (var memoryStream = new MemoryStream())
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                await JsonSerializer.SerializeAsync(memoryStream, carData, options);

                memoryStream.Position = 0;

                var saveResult = await FileSaver.Default.SaveAsync("queriedRdwData.txt", memoryStream,
                    cancellationToken: CancellationToken.None);

                if (saveResult.IsSuccessful)
                {
                    await Toast.Make($"The file was saved successfully to {saveResult.FilePath}").Show();
                }
                else
                {
                    await Toast.Make($"Error saving file: {saveResult.Exception.Message}").Show();
                }
            }
        });
        saveThread.Start();
        saveThread.Join();
        
        return Task.CompletedTask;
    }
}