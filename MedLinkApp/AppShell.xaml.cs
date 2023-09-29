namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        RegisterRoutingPages();
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
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

    }
}
