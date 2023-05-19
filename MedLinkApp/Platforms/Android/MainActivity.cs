using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MedLinkApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        UserActivityTrackService.Instance.StartSession();
    }
    public override void OnUserInteraction()
    {
        base.OnUserInteraction();

        UserActivityTrackService.Instance.RestartSession();
    }
}
