namespace MedLinkApp.Views;

public partial class OfferPage : ContentPage
{
	public OfferPage()
	{
		InitializeComponent();

        waitingForOfferFrame.BackgroundColor = Color.FromRgba(0, 0, 0, 195);

        BindingContext = new OfferViewModel();
	}

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}