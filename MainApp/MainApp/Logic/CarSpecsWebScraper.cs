using HtmlAgilityPack;
using MainApp.Models;

namespace MainApp.Logic;

public class CarSpecsWebScraper
{
    public ScrapedCarData GetCarSpecs(string licensePlate)
    {
        var web = new HtmlWeb();
        var doc = web.Load($"https://finnik.nl/kenteken/{licensePlate}");
        var imageNodes = doc.DocumentNode.QuerySelector("img.d-block");
        var rowNodes = doc.DocumentNode.QuerySelectorAll("div.row");
        
        var data = new Dictionary<string, string>();
        foreach (var row in rowNodes)
        {
            var childNodes = row.ChildNodes;

            foreach (var childNode in childNodes)
            {
                // Check if the child node is a div element
                if (childNode is not { Name: "div" }) continue;
                if (!childNode.HasClass("label")) continue;
                var labelText = childNode.InnerText.Replace("\n", "").Trim();

                // Get the next sibling div (assuming it's the value)
                var nextSibling = childNode.NextSibling;
                while (nextSibling != null)
                {
                    // Skip non-div elements
                    if (nextSibling is { Name: "div" } divNode && divNode.HasClass("value"))
                    {
                        var valueText = nextSibling.InnerText.Replace("\n", "").Trim();
                        data.TryAdd(labelText, valueText);
                        break; // Exit the loop once value div is found
                    }

                    nextSibling = nextSibling.NextSibling;
                }
            }
        }
        
        var imageUrl = HtmlEntity.DeEntitize(imageNodes.QuerySelector("img").Attributes["src"].Value);
        var horsePower = data["Vermogen"];
        var torque = data["Koppel"];
        var topSpeed = data["Topsnelheid"];
        var acceleration = data["Acceleratie 0-100"];

        return new ScrapedCarData(imageUrl, horsePower, torque, topSpeed, acceleration);
    }
}