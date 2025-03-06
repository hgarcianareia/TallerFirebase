using Microsoft.Maui.Controls.Internals;
using TallerFirebase.Services.Analytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase;

public partial class App : Application
{
    public App(IServiceProvider provider)
    {
        InitializeComponent();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        throw new Exception("OnSleep - Something went wrong");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}