namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();

		this.BindingContext = new AccountViewModel();
	}
}