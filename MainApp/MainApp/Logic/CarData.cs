using System.Text.Json.Serialization;

namespace MainApp.Logic;


[Serializable]
public class CarData
{
    [JsonPropertyName("kenteken")] 
    public string LicensePlate { get; set; }
    
    [JsonPropertyName("voertuigsoort")]
    public string VerhicleType { get; set; }
    
    [JsonPropertyName("merk")]
    public string Make { get; set; }
    
    [JsonPropertyName("handelsbenaming")]
    public string MakeType { get; set; }
    
    [JsonPropertyName("inrichting")]
    public string BodyConfiguration { get; set; }
    
    [JsonPropertyName("eerste_kleur")]
    public string PrimaryColor { get; set; }
    
    [JsonPropertyName("tweede_kleur")]
    public string SecondaryColor { get; set; }
}