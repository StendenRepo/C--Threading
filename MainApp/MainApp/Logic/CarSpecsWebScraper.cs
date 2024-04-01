using System.Diagnostics;
using HtmlAgilityPack;
using MainApp.Models;

namespace MainApp.Logic;

public class CarSpecsWebScraper
{
    public void GetCarSpecs()
    {
        var web = new HtmlWeb();
        var scrapedData = new ScrapedCarData();
        var doc = web.Load("https://finnik.nl/kenteken/s403xd");
        var imageNodes = doc.DocumentNode.QuerySelector("img.d-block");
        var rowNodes = doc.DocumentNode.QuerySelectorAll("div.row");

        var test = doc.DocumentNode.QuerySelector("div.col-6.col-sm-7.value").InnerText;
        
        var data = new Dictionary<string, string>();
        foreach (var row in rowNodes)
        {
            var childNodes = row.ChildNodes;

            foreach (var childNode in childNodes)
            {
                // Check if the child node is a div element
                if (childNode is not { Name: "div" }) continue;
                if (!childNode.HasClass("label")) continue;
                var labelText = childNode.InnerText.Replace(" ", "").Replace("\n", "");

                // Get the next sibling div (assuming it's the value)
                var nextSibling = childNode.NextSibling;
                while (nextSibling != null)
                {
                    // Skip non-div elements
                    if (nextSibling is { Name: "div" } divNode && divNode.HasClass("value"))
                    {
                        var valueText = nextSibling.InnerText.Replace(" ", "").Replace("\n", "");
                        data.TryAdd(labelText, valueText);
                        Console.WriteLine($"Label: {labelText}, Value: {valueText}");
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
        var acceleration = data["Acceleratie0-100"];
        
        Trace.WriteLine(horsePower);
        Trace.WriteLine(torque);
        Trace.WriteLine(topSpeed);
        Trace.WriteLine(acceleration);
    }
}