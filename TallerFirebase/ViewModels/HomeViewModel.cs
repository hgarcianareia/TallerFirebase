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
        var rnd = new Random();
        const string userId = "FirebaseUser";
        
        List<string> departamentos = new List<string>()
        {
            "Montevideo",
            "Canelones",
            "Maldonado",
            "Rocha",
            "Paysandú"
        };

        DateTime start = new DateTime(1975, 1, 1);
        int range = (DateTime.Today - start).Days;           
        var birthday = start.AddDays(rnd.Next(range));
        var today = DateTime.Today;
        var age = today.Year - birthday.Year;
        if (birthday.Date > today.AddYears(-age)) age--;
        
        _analytics.SetUserId(userId + $"- {rnd.Next(1, 5)}");
        _crashlytics.SetUserId(userId + $"- {rnd.Next(1, 5)}");

        _analytics.SetUserProperty("Edad",  age.ToString());
        _analytics.SetUserProperty("Ciudad", departamentos[rnd.Next(departamentos.Count)]);
        _analytics.SetUserProperty("Fecha de nacimiento", birthday.ToShortDateString());
    }

    [RelayCommand]
    private void LogNavigationEvent()
    {
        _analytics.LogEvent("Navegación", new Dictionary<string, object>()
        {
            { "Pantalla", "Pantalla principal" }
        });
    }

    [RelayCommand]
    private void SimulateCrash()
    {
        SimulateNullCrash();
    }

    [RelayCommand]
    private void RecordException()
    {
        try
        {
            SimulateNullCrash();
        }
        catch (Exception e)
        {
            var errorData = new Dictionary<string, object>
            {
                { "Pantalla", "Pantalla principal" },
                { "Accion", "Intento de compra" },
            };
            _crashlytics.RecordException(e, errorData);
        }
    }

    private void SimulateNullCrash()
    {
        throw new NullReferenceException("Simulando Null");
    }
}