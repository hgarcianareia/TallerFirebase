using Firebase.Crashlytics;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase.Platforms.Android.Services;

public class FirebaseCrashlyticsService : IFirebaseCrashlyticsService
{
    private readonly FirebaseCrashlytics _firebaseCrashlytics;

    public FirebaseCrashlyticsService()
    {
        _firebaseCrashlytics = FirebaseCrashlytics.Instance;
    }

    // Log a message that's included in the next fatal, non-fatal or ANR report.
    public void Log(string message)
    {
        _firebaseCrashlytics.Log(message);
    }

    // Record a non-fatal report to send to Crashlytics
    public void RecordException(Exception exception, Dictionary<string, object>? parameters = null)
    {
        try
        {
            if (_firebaseCrashlytics == null)
            {
                Console.WriteLine("[Firebase] Advertencia: Firebase Crashlytics no está inicializado.");
                return;
            }

            _firebaseCrashlytics.SetCustomKey("ExceptionMessage", exception.Message);

            if (exception.InnerException != null)
            {
                _firebaseCrashlytics.SetCustomKey("InnerException", exception.InnerException.Message);
            }

            if (parameters?.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    _firebaseCrashlytics.SetCustomKey(parameter.Key, parameter.Value?.ToString() ?? "null");
                }
            }

            _firebaseCrashlytics.RecordException(Java.Lang.Throwable.FromException(exception));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Firebase] Error al registrar excepción: {ex}");
        }
    }

    // Records a user ID (identifier) that's associated with subsequent fatal, non-fatal, and ANR reports.
    public void SetUserId(string userId)
    {
        _firebaseCrashlytics.SetUserId(userId);
    }
}