namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

        this.BindingContext = new HomeViewModel();
    }
    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        return false;
    }
}