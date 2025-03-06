using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Firebase.Crashlytics;
using TallerFirebase.Services.Analytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase.ViewModels;

public partial class PageTwoViewModel : ObservableObject
{
    private readonly IFirebaseCrashlyticsService _crashlytics;
    private readonly IFirebaseAnalyticsService _analytics;

    public PageTwoViewModel(IFirebaseAnalyticsService analytics, IFirebaseCrashlyticsService crashlytics)
    {
        _analytics = analytics;
        _crashlytics = crashlytics;
    }

    [RelayCommand]
    private void SimulateCrash()
    {
        var number = new Random().Next(1, 10);
        DivideByZeroError(number);
    }

    [RelayCommand]
    private void LogNavigationEvent()
    {
        _analytics.LogEvent("Navegación", new Dictionary<string, object>()
        {
            { "Pantalla", "Pantalla 2" }
        });
    }

    [RelayCommand]
    private void RecordException()
    {
        var number = new Random().Next(1, 10);
        
        try
        {
            DivideByZeroError(number);
        }
        catch (Exception e)
        {
            var errorData = new Dictionary<string, object>
            {
                { "Pantalla", "Pantalla secundaria" },
                { "Accion", "Dividir entre 0" },
                { "Monto", number }
            };

            _crashlytics.RecordException(e, errorData);
        }
    }

    private static void DivideByZeroError(int number)
    {
        var zero = 0;
        var div = number / zero;
    }
}