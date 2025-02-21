using Microsoft.Maui.Controls.Internals;
using TallerFirebase.Services.Analytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase;

public partial class App : Application
{
    private readonly IFirebaseAnalyticsService _analytics;
    private readonly IFirebaseCrashlyticsService _crashlytics;

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        _analytics = provider.GetRequiredService<IFirebaseAnalyticsService>();
        _crashlytics = provider.GetRequiredService<IFirebaseCrashlyticsService>();
        // AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            var currentTab = Shell.Current.CurrentItem;
            _crashlytics.Log($"Current Tab: {currentTab.Title}");
        }
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