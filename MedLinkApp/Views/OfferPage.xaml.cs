namespace MedLinkApp.Views;

public partial class OfferPage : ContentPage
{
	public OfferPage()
	{
		InitializeComponent();

		BindingContext = new OfferViewModel();
	}

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}