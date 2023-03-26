namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));
		Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
    }
}
