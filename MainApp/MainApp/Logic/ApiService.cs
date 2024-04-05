using System.Diagnostics;
using System.Text.Json;
using MainApp.Models;

namespace MainApp.Logic;

public class ApiService : IApiService
{
    private readonly HttpClient _client = new();
    private const string AppToken = "Fyr47PwDKOX44RykT02Tc0Fky";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Queries rdw data via their api by specifying a license plate.
    /// </summary>
    /// <param name="licensePlate">The license plate.</param>
    /// <returns>A task with <see cref="CarData"/>.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<CarData> QueryRdwDataAsync(string licensePlate)
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
                
                // Deserialize data to a CarData object
                cars = JsonSerializer.Deserialize<List<CarData>>(content, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error {0}", ex.Message);
            throw new Exception();
        }

        return cars.FirstOrDefault();
    }
    
    /// <summary>
    /// Queries 500000 in parallel
    /// </summary>
    /// <returns></returns>
    public async Task<List<CarData>> QueryAllRdwDataInParallel()
    {
        _client.DefaultRequestHeaders.Add("X-App-Token", AppToken);
        var cars = new List<CarData>();
        var tasks = new List<Task<List<CarData>>>();
        
        // Fetch 500000 records from rdw
        for (var x = 0; x < 500000; x += 1000)
        {
            tasks.Add(GetRdwDataAsync(x));
        }
    
        var results = await Task.WhenAll(tasks);
        foreach (var result in results)
        {
            cars.AddRange(result);
        }
        
        return cars;
    }

    /// <summary>
    /// Queries 1000 rdw records from the offset point.
    /// </summary>
    /// <param name="offset">The offset of where to start.</param>
    /// <returns>A Task containing a list with <see cref="CarData"/>.</returns>
    private async Task<List<CarData>> GetRdwDataAsync(object offset)
    {
        var result = new List<CarData>();
        var x = (int)offset;
        var uri = new Uri($"https://opendata.rdw.nl/resource/m9d7-ebf2.json?$offset={x}");
            
        try
        {
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<CarData>>(content, _serializerOptions);
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error {0}", ex.Message);
            result = [];
        } 
        return result;
    }
}