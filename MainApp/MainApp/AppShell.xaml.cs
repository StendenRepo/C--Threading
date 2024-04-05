using MainApp.Views;

namespace MainApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(TuningResultPage), typeof(TuningResultPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}