namespace MedLinkApp.Views;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage()
	{
		InitializeComponent();

		BindingContext = new ResetPasswordViewModel();
	}
}