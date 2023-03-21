namespace MedLinkApp;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
