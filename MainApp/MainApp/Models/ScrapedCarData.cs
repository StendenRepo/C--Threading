namespace MainApp.Models;

public class ScrapedCarData(
    string? imageUrl,
    string? horsePower,
    string? torque,
    string? topSpeed,
    string? acceleration)
{
    public string? ImageUrl { get; set; } = imageUrl;
    public string? HorsePower { get; set; } = horsePower;
    public string? Torque { get; set; } = torque;
    public string? TopSpeed { get; set; } = topSpeed;
    public string? Acceleration { get; set; } = acceleration;
}