using Plugin.Firebase.Analytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase.Services.Analytics;

public class FirebaseAnalyticsService : IFirebaseAnalyticsService
{
    private readonly IFirebaseAnalytics _firebaseAnalytics;

    public FirebaseAnalyticsService(IFirebaseAnalytics firebaseAnalytics)
    {
        _firebaseAnalytics = firebaseAnalytics;
    }

    public void LogEvent(string eventName)
    {
        LogEvent(eventName, null);
    }

    // Logs an app event. The event can have up to 25 parameters.
    // Events with the same name must have the same parameters. Up to 500 event names are supported.
    // Using predefined events and/ or parameters is recommended for optimal reporting.
    public void LogEvent(string eventName, Dictionary<string, object>? eventParams)
    {
        var stringDict = eventParams?.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)(kvp.Value?.ToString() ?? "null")
        );

        _firebaseAnalytics.LogEvent(eventName, stringDict);
    } 

    // Sets the user ID property.
    public void SetUserId(string userId)
    {
        _firebaseAnalytics.SetUserId(userId);
    }

    // Sets a user property to a given value. Up to 25 user property names are supported.
    // Once set, user property values persist throughout the app lifecycle and across sessions.
    public void SetUserProperty(string key, string value)
    {
        _firebaseAnalytics.SetUserProperty(key, value);
    }
}