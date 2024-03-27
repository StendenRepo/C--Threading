using System.Diagnostics;
using System.Text.Json;

namespace MainApp.Logic;

public class ApiService
{
    private readonly HttpClient _client = new();

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<CarData> QueryData()
    {
        _client.DefaultRequestHeaders.Add("X-App-Token", "Fyr47PwDKOX44RykT02Tc0Fky");
 
        var cars = new List<CarData>();
        var uri = new Uri("https://opendata.rdw.nl/resource/m9d7-ebf2.json?kenteken=S403XD");
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
}