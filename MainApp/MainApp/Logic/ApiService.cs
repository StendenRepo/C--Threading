using System.Diagnostics;
using System.Text.Json;

namespace MainApp.Logic;

public class ApiService : IApiService
{
    private readonly HttpClient _client = new();
    private const string AppToken = "Fyr47PwDKOX44RykT02Tc0Fky";
    private const string GoogleApiKey = "AIzaSyBNJ0tuiia8ksHRtvUtbOsYrg-ppHgVmZ0";
    private const string SearchEngineId = "c78e4e46df89e4d8a";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<CarData> QueryRdwData(string licensePlate)
    {
        _client.DefaultRequestHeaders.Add("X-App-Token", AppToken);
        var cars = new List<CarData>();

        var uri = new Uri($"https://opendata.rdw.nl/resource/m9d7-ebf2.json?kenteken={licensePlate}");
        try
        {
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                cars = JsonSerializer.Deserialize<List<CarData>>(content, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        return cars.FirstOrDefault();
    }

    public async Task SearchImage()
    {
        // _client.DefaultRequestHeaders.Add("X-App-Token", AppToken);
        var cars = new List<CarData>();

        var uri = new Uri($"https://www.googleapis.com/customsearch/v1?key={GoogleApiKey}&cx={SearchEngineId}&searchType=image&imgDominantColor=white&q=volkswagen+UP!");
        try
        {
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                cars = JsonSerializer.Deserialize<List<CarData>>(content, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }
}