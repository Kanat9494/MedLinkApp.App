namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AudioMessagePage : ContentPage
{
	public AudioMessagePage()
	{
		InitializeComponent();

		BindingContext = new AudioMessageViewModel();
	}
}