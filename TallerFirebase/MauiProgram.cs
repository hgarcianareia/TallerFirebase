#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
using TallerFirebase.Platforms.Android.Services;
using TallerFirebase.Services.Crashlytics;
#elif IOS
using Plugin.Firebase.Core.Platforms.iOS;
using TallerFirebase.Services.Crashlytics;
using TallerFirebase.Platforms.iOS.Services;
#endif
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Analytics;
using Plugin.Firebase.Crashlytics;
using TallerFirebase.Services.Analytics;
using TallerFirebase.ViewModels;
using TallerFirebase.Views;

namespace TallerFirebase;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterFirebaseServices();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomeView>();
        builder.Services.AddTransient<PageTwoViewModel>();
        builder.Services.AddTransient<PageTwoView>();

        // builder.Services.AddSingleton<IFirebaseCrashlytics>(_ => CrossFirebaseCrashlytics.Current);
        builder.Services.AddSingleton<IFirebaseAnalytics>(_ => CrossFirebaseAnalytics.Current);
        builder.Services.AddSingleton<IFirebaseAnalyticsService, FirebaseAnalyticsService>();
        
#if ANDROID
        builder.Services.AddSingleton<IFirebaseCrashlyticsService, FirebaseCrashlyticsService>();
#elif IOS
        builder.Services.AddSingleton<IFirebaseCrashlyticsService, FirebaseCrashlyticsService>();
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((app, launchOptions) =>
            {
                CrossFirebase.Initialize();
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
                return false;
            }));
#else
            events.AddAndroid(android => android.OnCreate((activity, bundle) =>
            {
                CrossFirebase.Initialize(activity);
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
                FirebaseAnalyticsImplementation.Initialize(activity);
            }));
#endif
        });

        return builder;
    }
}