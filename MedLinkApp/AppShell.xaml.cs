namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        RegisterPages();
    }

	private void RegisterPages()
	{
        Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));
        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
        Routing.RegisterRoute(nameof(AudioMessagePage), typeof(AudioMessagePage));
    }
}
