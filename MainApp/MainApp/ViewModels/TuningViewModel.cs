using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Logic;
using MainApp.Models;
using Microcharts;
using SkiaSharp;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? _plate;
    [ObservableProperty] private CarData? _carData;
    [ObservableProperty] private TuningResult? _tuningResult;
    [ObservableProperty] private Chart _horsePowerChart;
    [ObservableProperty] private Chart _torqueChart;
    [ObservableProperty] private bool _isLoading;
    private readonly IApiService _apiService = new ApiService();
    private readonly CarSpecsWebScraper _webScraper = new();

    [RelayCommand]
    private async Task Submit(string licensePlate)
    {
        if (licensePlate is not { Length: 6 })
        {
            await Shell.Current.DisplayAlert("Er ging iets fout", "Vul een geldig kenteken in. Bijvoorbeeld: S403XF", "Ok");
            return;
        }
        await Shell.Current.GoToAsync($"TuningResultPage?license={licensePlate}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Plate = query["license"] as string;
    }

    /// <summary>
    /// Fetches the car data corresponding to the license plate.
    /// </summary>
    /// <param name="licensePlate">The license plate.</param>
    /// <returns>A task with <see cref="CarData"/>.</returns>
    private async Task<CarData> FetchCarData(string licensePlate)
    {
        var carData = await _apiService.QueryRdwDataAsync(licensePlate);

        carData.ScrapedCarData = await _webScraper.GetCarSpecs(licensePlate);

        if (carData.ScrapedCarData is { HorsePower: not null, Torque: not null })
        {
            TuningResult = new TuningResult(carData.ScrapedCarData.HorsePower, carData.ScrapedCarData.Torque);
        }
        return carData;
    }

    /// <summary>
    /// When loading TuningResultPage this method gets executed.
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        try
        {
            if (Plate != null)
            {
                CarData = await FetchCarData(Plate);
                GenerateCharts();
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Er ging iets fout", "Kenteken kon niet gevonden worden", "Ga terug");
            await Shell.Current.GoToAsync("///MainPage");
        }
    }

    /// <summary>
    /// Generates the charts for horsepower and torque
    /// </summary>
    private void GenerateCharts()
    {
        // Create data entries to display in the chart
        var horsePowerEntries = new[]
        {
            new ChartEntry((float)TuningResult!.HorsePowerBeforeTuning)
            {
                Label = "Voor tuning",
                ValueLabel = TuningResult.HorsePowerBeforeTuning.ToString(CultureInfo.InvariantCulture),
                Color = SKColor.Parse("#266488")
            },
            new ChartEntry((float)TuningResult!.HorsePowerAfterTuning)
            {
                Label = "Na tuning",
                ValueLabel = TuningResult.HorsePowerAfterTuning.ToString(CultureInfo.InvariantCulture),
                Color = SKColor.Parse("#68b9c0")
            }
        };
        
        var torqueEntries = new[]
        {
            new ChartEntry((float)TuningResult!.TorqueBeforeTuning)
            {
                Label = "Voor tuning",
                ValueLabel = TuningResult.TorqueBeforeTuning.ToString(CultureInfo.InvariantCulture),
                Color = SKColor.Parse("#266488")
            },
            new ChartEntry((float)TuningResult!.TorqueAfterTuning)
            {
                Label = "Na tuning",
                ValueLabel = TuningResult.TorqueAfterTuning.ToString(CultureInfo.InvariantCulture),
                Color = SKColor.Parse("#68b9c0")
            }
        };
        
        HorsePowerChart = new BarChart()
        {
            Entries = horsePowerEntries,
            CornerRadius = 10,
            MaxValue = (float)TuningResult.HorsePowerAfterTuning + 100,
            BackgroundColor = SKColor.Parse("#fae39d"),
            ValueLabelOrientation = Orientation.Horizontal,
            LabelOrientation = Orientation.Horizontal,
            LabelTextSize = 15,
            ShowYAxisLines = true,
        };
        TorqueChart = new DonutChart()
        {
            Entries = torqueEntries,
            MaxValue = (float)TuningResult.TorqueAfterTuning + 100,
            BackgroundColor = SKColor.Parse("#fae39d"),
        };
    }

    /// <summary>
    /// Exports RDW data.
    /// </summary>
    [RelayCommand]
    private async Task ExportData()
    {
        IsLoading = true;
        var carData = await _apiService.QueryAllRdwDataInParallel();
        await SaveCarDataToFileAsync(carData);
    }
    
    /// <summary>
    /// Saves all rdw data to a text file.
    /// </summary>
    /// <param name="carData">A <see cref="CarData"/>.</param>
    /// <returns>A Task</returns>
    private Task SaveCarDataToFileAsync(List<CarData> carData)
    {
        // Create a new thread for writing data to text file.
        var saveThread = new Thread(async () =>
        {
            using (var memoryStream = new MemoryStream())
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                await JsonSerializer.SerializeAsync(memoryStream, carData, options);

                memoryStream.Position = 0;

                // Save the stream to a text file
                var saveResult = await FileSaver.Default.SaveAsync("queriedRdwData.txt", memoryStream,
                    cancellationToken: CancellationToken.None);

                // Provide message based on outcome
                if (saveResult.IsSuccessful)
                {
                    await Toast.Make($"The file was saved successfully to {saveResult.FilePath}").Show();
                }
                else
                {
                    await Toast.Make($"Error saving file: {saveResult.Exception.Message}").Show();
                }
            }
            IsLoading = false;
        });
        saveThread.Start();
        saveThread.Join();
        return Task.CompletedTask;
    }
}