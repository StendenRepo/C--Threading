using MainApp.Logic;

namespace MainApp;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnSubmitClicked(object? sender, EventArgs e)
    {
        var service = new ApiService();
        _ = service.QueryData();
    }
}