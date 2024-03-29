using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MainApp.ViewModels;

public partial class TuningViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? _plate;
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Plate = query["license"] as string;
        Trace.WriteLine(Plate);
    }
}