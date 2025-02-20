using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Firebase.Crashlytics;
using TallerFirebase.Services.Analytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IFirebaseCrashlyticsService _crashlytics;
    private readonly IFirebaseAnalyticsService _analytics;

    public HomeViewModel(IFirebaseCrashlyticsService crashlytics, IFirebaseAnalyticsService analytics)
    {
        _crashlytics = crashlytics;
        _analytics = analytics;
    }

    [RelayCommand]
    private void SetUserProps()
    {
        const string userId = "FirebaseUser1234";

        _analytics.SetUserId(userId);
        _crashlytics.SetUserId(userId);

        _analytics.SetUserProperty("Edad", "30");
        _analytics.SetUserProperty("Ciudad", "Montevideo");
        _analytics.SetUserProperty("Fecha de nacimiento", "14/10/1994");
    }

    [RelayCommand]
    private void SimulateCrash()
    {
        _crashlytics.Log("Log - Crash simulado");
        throw new NullReferenceException("Crash simulado");
    }

    [RelayCommand]
    private void RecordException()
    {
        try
        {
            throw new NullReferenceException($"Algo dio null: {DateTime.Now.TimeOfDay.ToString()}");
        }
        catch (Exception e)
        {
            _crashlytics.Log("Log - Error manejado");
            var errorData = new Dictionary<string, object>
            {
                { "Pantalla", "Pantalla principal" },
                { "Accion", "Intento de compra" },
                { "Monto", 99.99 }
            };
            _crashlytics.RecordException(e, errorData);
        }
    }

    [RelayCommand]
    private void LogEvent()
    {
        _analytics.LogEvent("Evento_sin_parámetros");

        _analytics.LogEvent("Evento_con_parámetros", new Dictionary<string, object>()
        {
            { "Hora", DateTime.Now.TimeOfDay },
            { "Int", 4 },
            { "Boolean", true },
        });
    }
}