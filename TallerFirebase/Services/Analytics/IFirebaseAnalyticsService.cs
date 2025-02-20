namespace TallerFirebase.Services.Analytics;

public interface IFirebaseAnalyticsService
{
    void LogEvent(string eventName);
    void LogEvent(string eventName, Dictionary<string, object> eventParams);
    void SetUserId(string userId);
    void SetUserProperty(string key, string value);
}