using System.Text.Json.Serialization;

namespace MainApp.Models;


[Serializable]
public class CarData
{
    [JsonPropertyName("kenteken")] 
    public string? LicensePlate { get; set; }
    
    [JsonPropertyName("voertuigsoort")]
    public string? VehicleType { get; set; }
    
    [JsonPropertyName("merk")]
    public string? Make { get; set; }
    
    [JsonPropertyName("handelsbenaming")]
    public string? MakeType { get; set; }
    
    [JsonPropertyName("inrichting")]
    public string? BodyConfiguration { get; set; }
    
    [JsonPropertyName("eerste_kleur")]
    public string? PrimaryColor { get; set; }
    
    public ScrapedCarData? ScrapedCarData { get; set; }

    public string FullName => Make + " " + MakeType;
}