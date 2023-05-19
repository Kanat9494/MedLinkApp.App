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

        //SessionManager.Instance.SessionDuration = TimeSpan.FromSeconds(10);
        //SessionManager.Instance.OnSessionExpired = HandleSessionExpired;
    }
    public override void OnUserInteraction()
    {
        base.OnUserInteraction();

        //SessionManager.Instance.ExtendSession();
    }

    //async void HandleSessionExpired(object sender, EventArgs e)
    //    //=> await SessionManager.Instance.LogoutAsync();
}
