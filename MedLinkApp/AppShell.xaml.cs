namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        RegisterRoutingPages();

        _idleTimeoutService = new IdleTimeoutService();
    }

    private IdleTimeoutService _idleTimeoutService;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        SubscribeToNavigationEvents();
        StartIdleTimer();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        UnsubscribeFromNavigationEvents();
        StopIdleTimer();
    }

    private void SubscribeToNavigationEvents()
    {
        Navigating += HandleNavigating;
        Navigated += HandleNavigated;
    }

    private void UnsubscribeFromNavigationEvents()
    {
        Navigating -= HandleNavigating;
        Navigated -= HandleNavigated;
    }

    private void HandleNavigating(object sender, ShellNavigatingEventArgs e)
    {
        StopIdleTimer();
    }

    private void HandleNavigated(object sender, ShellNavigatedEventArgs e)
    {
        StartIdleTimer();
    }

    private void StartIdleTimer()
    {
        _idleTimeoutService.StartTimer();
    }

    private void StopIdleTimer()
    {
        _idleTimeoutService.ResetTimer();
    }

    private void RegisterRoutingPages()
	{
        Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));
        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
        Routing.RegisterRoute(nameof(AudioMessagePage), typeof(AudioMessagePage));
        Routing.RegisterRoute(nameof(ImageBrowsePage), typeof(ImageBrowsePage));
        Routing.RegisterRoute(nameof(OfferPage), typeof(OfferPage));
        Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
    }
}
