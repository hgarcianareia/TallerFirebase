using System;
using System.Collections.Generic;

namespace TallerFirebase.Services.Crashlytics;

public interface IFirebaseCrashlyticsService
{
    void Log(string message);
    void RecordException(Exception exception, Dictionary<string, object>? parameters = null);
    void SetUserId(string userId);
}