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
    private void ChangeUserProp()
    {
        _analytics.SetUserProperty("Edad", "28");
        _analytics.SetUserProperty("Ciudad", "Canelones");
    }

    [RelayCommand]
    private void Crash()
    {
        DivideByZeroError();
    }


    [RelayCommand]
    private void CustomError()
    {
        try
        {
            DivideByZeroError();
        }
        catch (Exception e)
        {
            var errorData = new Dictionary<string, object>
            {
                { "Pantalla", "Pantalla secundaria" },
                { "Accion", "Intento de compra" },
                { "Monto", 99.99 }
            };

            _crashlytics.RecordException(e, errorData);
        }
    }
    
    [RelayCommand]
    private void ErrorSDK()
    {
        try
        {
            DivideByZeroError();
        }
        catch (Exception e)
        {
            var errorData = new Dictionary<string, object>
            {
                { "Pantalla", "Pantalla secundaria" },
                { "Accion", "Intento de compra" },
                { "Monto", 99.99 }
            };

            CrossFirebaseCrashlytics.Current.RecordException(e);
        }
    }

    private static void DivideByZeroError()
    {
        var zero = 0;

        var div = 8 / zero;
    }
}