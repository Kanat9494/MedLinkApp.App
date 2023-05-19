namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(CustomEntry), (handler, view) =>
		{
#if __ANDROID__
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
			handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
			handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
		});
	}

	private const int IDLE_TIMEOUT_SECONDS = 5000;
	private static readonly TimeSpan IdleTimeout = TimeSpan.FromSeconds(IDLE_TIMEOUT_SECONDS);
	private static bool _isUserActive = true;
	private static DateTime _lastActivityTime;

    protected override void OnStart()
    {
		base.OnStart();

		_lastActivityTime = DateTime.Now;

		App.Current.UserAppThemeChanged += 
    }
}
