using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Crashlytics;
using Foundation;
using TallerFirebase.Services.Crashlytics;

namespace TallerFirebase.Platforms.iOS.Services;

public class FirebaseCrashlyticsService : IFirebaseCrashlyticsService
{
    private readonly Crashlytics _firebaseCrashlytics;

    public FirebaseCrashlyticsService()
    {
        _firebaseCrashlytics = Crashlytics.SharedInstance;
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

            var errorInfo = new Dictionary<object, object>
            {
                { NSError.LocalizedDescriptionKey, exception.Message },
            };

            if (exception.InnerException != null)
            {
                errorInfo.Add(new NSString("InnerException"), exception.InnerException.Message);
            }

            var error = ToNSError(exception, exception.HResult);
            // var error = new NSError(new NSString("NonFatalError"),
            //     exception.HResult,  
            //     NSDictionary.FromObjectsAndKeys(errorInfo.Values.ToArray(), errorInfo.Keys.ToArray(), errorInfo.Keys.Count));

            if (parameters?.Count > 0)
            {
                var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(
                    parameters.Values.Select(v => new NSString(v?.ToString() ?? "null")).ToArray(),
                    parameters.Keys.Select(k => new NSString(k)).ToArray()
                );

                _firebaseCrashlytics.SetCustomKeysAndValues(parametersDictionary);
            }

            _firebaseCrashlytics.RecordError(error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Firebase] Error al registrar excepción: {ex}");
        }
    }
    
    public static NSError ToNSError(Exception ex, int errorCode = -1)
    {
        if (ex == null)
            return null;

        var userInfo = new NSMutableDictionary
        {
            { new NSString("Description"), new NSString(ex.Message) },
            { new NSString("StackTrace"), new NSAttributedString(ex.StackTrace ?? "No stack trace available") },
            { new NSString("InnerException"), new NSString(ex.InnerException?.Message ?? "No inner exception available") },
        };

        return new NSError(new NSString("NonFatalError"), errorCode, userInfo);
    }

    // Records a user ID (identifier) that's associated with subsequent fatal, non-fatal, and ANR reports.
    public void SetUserId(string userId)
    {
        _firebaseCrashlytics.SetUserId(userId);
    }
}